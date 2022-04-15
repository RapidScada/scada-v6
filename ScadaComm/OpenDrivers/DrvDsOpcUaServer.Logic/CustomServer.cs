// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Server;
using Scada.Lang;
using Scada.Log;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Scada.Comm.Drivers.DrvDsOpcUaServer.Logic
{
    /// <summary>
    /// Implements OPC UA server for Rapid SCADA.
    /// <para>Реализует сервер OPC UA для Rapid SCADA.</para>
    /// </summary>
    internal class CustomServer : StandardServer
    {
        private readonly ICommContext commContext; // the application context
        private readonly OpcUaServerDSO options;   // the data source options
        private readonly ILog log;                 // the data source log

        private ICertificateValidator certificateValidator;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CustomServer(ICommContext commContext, OpcUaServerDSO options, ILog log)
        {
            this.commContext = commContext ?? throw new ArgumentNullException(nameof(commContext));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            certificateValidator = null;
            NodeManager = null;
        }


        /// <summary>
        /// Gets the node manager.
        /// </summary>
        public NodeManager NodeManager { get; private set; }


        /// <summary>
        /// Creates the objects used to validate the user identity tokens supported by the server.
        /// </summary>
        private void CreateUserIdentityValidators(ApplicationConfiguration configuration)
        {
            foreach (UserTokenPolicy policy in configuration.ServerConfiguration.UserTokenPolicies)
            {
                // create a validator for a certificate token policy
                if (policy.TokenType == UserTokenType.Certificate)
                {
                    // check if user certificate trust lists are specified in configuration
                    SecurityConfiguration securityConfiguration = configuration.SecurityConfiguration;

                    if (securityConfiguration.TrustedUserCertificates != null &&
                        securityConfiguration.UserIssuerCertificates != null)
                    {
                        CertificateValidator validator = new CertificateValidator();
                        validator.Update(securityConfiguration).Wait();
                        validator.Update(
                            securityConfiguration.UserIssuerCertificates,
                            securityConfiguration.TrustedUserCertificates,
                            securityConfiguration.RejectedCertificateStore);

                        // set custom validator for user certificates
                        certificateValidator = validator.GetChannelValidator();
                    }
                }
            }
        }

        /// <summary>
        /// Called when a client tries to change its user identity.
        /// </summary>
        private void SessionManager_ImpersonateUser(Session session, ImpersonateEventArgs args)
        {
            if (args.NewIdentity is UserNameIdentityToken userNameToken)
            {
                // check for a user name token
                args.Identity = VerifyPassword(userNameToken);

                // set AuthenticatedUser role for accepted user/password authentication
                args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_AuthenticatedUser);

                if (args.Identity is SystemConfigurationIdentity)
                {
                    // set ConfigureAdmin role for user with permission to configure server
                    args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_ConfigureAdmin);
                    args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_SecurityAdmin);
                }
            }
            else if (args.NewIdentity is X509IdentityToken x509Token)
            {
                // check for x509 user token
                VerifyUserTokenCertificate(x509Token.Certificate);
                args.Identity = new UserIdentity(x509Token);

                // set AuthenticatedUser role for accepted certificate authentication
                args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_AuthenticatedUser);
            }
            else if (string.IsNullOrEmpty(options.Username))
            {
                // allow anonymous authentication and set Anonymous role for this authentication
                args.Identity = new UserIdentity();
                args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_Anonymous);

                log.WriteAction(Locale.IsRussian ?
                    "Анонимная аутентификация" :
                    "Anonymous authentication");
            }
            else
            {
                string msg = Locale.IsRussian ?
                    "Анонимная аутентификация не допускается" :
                    "Anonymous authentication is not allowed";
                log.WriteError(msg);
                throw new ServiceResultException(StatusCodes.BadIdentityTokenRejected, msg);
            }
        }

        /// <summary>
        /// Validates the password for a username token.
        /// </summary>
        private IUserIdentity VerifyPassword(UserNameIdentityToken userNameToken)
        {
            string username = userNameToken.UserName;
            string password = userNameToken.DecryptedPassword;
            string msg;

            if (string.IsNullOrEmpty(username))
            {
                msg = Locale.IsRussian ?
                    "Пустое имя пользователя не допускается" :
                    "Empty username is not allowed";
                log.WriteError(msg);
                throw new ServiceResultException(StatusCodes.BadIdentityTokenInvalid, msg);
            }

            if (string.IsNullOrEmpty(password))
            {
                msg = Locale.IsRussian ?
                    "Пустой пароль не допускается" :
                    "Empty password is not allowed";
                log.WriteError(msg);
                throw new ServiceResultException(StatusCodes.BadIdentityTokenRejected, msg);
            }

            // user with permission to configure server
            //if (userName == "admin" && password == "scada")
            //    return new SystemConfigurationIdentity(new UserIdentity(userNameToken));

            // standard user
            if (username == options.Username && password == options.Password)
            {
                log.WriteAction(Locale.IsRussian ?
                    "Пользователь '{0}' принят" :
                    "User '{0}' accepted", username);
                return new UserIdentity(userNameToken);
            }

            msg = string.Format(Locale.IsRussian ?
                "Неверное имя пользователя или пароль для '{0}'" :
                "Invalid username or password for '{0}'", username);
            log.WriteError(msg);
            throw new ServiceResultException(new ServiceResult(StatusCodes.BadUserAccessDenied, "InvalidPassword", 
                LoadServerProperties().ProductUri, new LocalizedText(msg)));
        }

        /// <summary>
        /// Verifies that a certificate user token is trusted.
        /// </summary>
        private void VerifyUserTokenCertificate(X509Certificate2 certificate)
        {
            try
            {
                (certificateValidator ?? CertificateValidator).Validate(certificate);

                log.WriteAction(Locale.IsRussian ?
                    "Сертификат X.509 принят: {0}" :
                    "X.509 certificate accepted: {0}", certificate.FriendlyName);
            }
            catch (Exception ex)
            {
                string msg;
                StatusCode statusCode;
                string symbolicId;

                if (ex is ServiceResultException se && se.StatusCode == StatusCodes.BadCertificateUseNotAllowed)
                {
                    msg = string.Format(Locale.IsRussian ?
                        "Некорректный сертификат X.509: {0}" :
                        "Invalid X.509 certificate: {0}", certificate.Subject);
                    statusCode = StatusCodes.BadIdentityTokenInvalid;
                    symbolicId = "InvalidCertificate";
                }
                else
                {
                    msg = string.Format(Locale.IsRussian ?
                        "Недоверенный сертификат X.509: {0}" :
                        "Untrusted X.509 certificate: {0}", certificate.Subject);
                    statusCode = StatusCodes.BadIdentityTokenRejected;
                    symbolicId = "UntrustedCertificate";
                }

                log.WriteError(msg);
                throw new ServiceResultException(new ServiceResult(statusCode, symbolicId, 
                    LoadServerProperties().ProductUri, new LocalizedText(msg)));
            }
        }


        /// <summary>
        /// Creates the master node manager for the server.
        /// </summary>
        protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            NodeManager = new NodeManager(server, configuration, commContext, options, log);
            return new MasterNodeManager(server, configuration, null, new INodeManager[] { NodeManager });
        }

        /// <summary>
        /// Called before the server starts.
        /// </summary>
        protected override void OnServerStarting(ApplicationConfiguration configuration)
        {
            base.OnServerStarting(configuration);

            // it is up to the application to decide how to validate user identity tokens,
            // this function creates validator for X509 identity tokens
            CreateUserIdentityValidators(configuration);
        }

        /// <summary>
        /// Called after the server has been started.
        /// </summary>
        protected override void OnServerStarted(IServerInternal server)
        {
            base.OnServerStarted(server);

            // request notifications when the user identity is changed,
            // all valid users are accepted by default
            server.SessionManager.ImpersonateUser += new ImpersonateEventHandler(SessionManager_ImpersonateUser);
        }
    }
}
