# Rapid SCADA Docker Deployment

Docker containers for Rapid SCADA v6 - production-ready SCADA system.

## Quick Start

```bash
cd docker
docker compose up -d
```

Access the web interface at `http://localhost:5000`

**Note:** For a fully functional system, mount your SCADA project files. See [Configuration](#configuration) below.

## Services

| Service | Port | Description |
|---------|------|-------------|
| **scada-web** | 5000 | Web interface |
| **scada-server** | 10000 | Data processing engine |
| **scada-comm** | - | Device communication |
| **scada-agent** | 10002 | Remote management |
| **postgres** | 5432 | Database (optional) |

## Configuration

### What You'll See Initially

After `docker compose up`, all containers start successfully but the web shows:
- ✅ Containers: healthy
- ⚠️ Web message: "The application is not ready"

This is expected - SCADA needs project configuration files.

### Adding Your Project

Edit `docker-compose.yml` and uncomment line 83:

```yaml
services:
  scada-server:
    volumes:
      - ../Projects/HelloWorld/BaseXML:/app/BaseXML:ro
```

Restart:
```bash
docker compose up -d
```

Your project will now load in the web interface.

### Using Your Own Project

Replace with your project path:
```yaml
volumes:
  - /path/to/your/project/BaseXML:/app/BaseXML:ro
```

## Development

For local development with faster iteration:

```bash
docker compose -f docker-compose.dev.yml up
```

**Features:**
- Host-mounted logs in `./dev-data/`
- Lower resource limits  
- No auto-restart
- Debug logging

**Optional services:**
```bash
# Include Agent
docker compose -f docker-compose.dev.yml --profile full up

# Include PostgreSQL
docker compose -f docker-compose.dev.yml --profile with-db up
```

## Common Commands

```bash
# View logs
docker compose logs -f

# Restart a service
docker compose restart scada-server

# Stop everything
docker compose down

# Rebuild after code changes
docker compose build --no-cache
```

## Production Deployment

### 1. Update PostgreSQL Password

Edit `docker-compose.yml`:
```yaml
environment:
  POSTGRES_PASSWORD: your_secure_password_here
```

### 2. Mount Your Project

```yaml
services:
  scada-server:
    volumes:
      - /path/to/your/project/BaseXML:/app/BaseXML:ro
```

### 3. Deploy

```bash
docker compose up -d
```

### Behind Reverse Proxy (Nginx)

```nginx
server {
    listen 80;
    server_name scada.example.com;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }
}
```

## Troubleshooting

### "The application is not ready"

Mount your project's BaseXML directory - see [Configuration](#configuration).

### "Server started with errors"

Same as above - server needs project configuration files.

### Permission Errors

```bash
sudo chown -R 1000:1000 ./dev-data/
```

### Can't Connect to Services

```bash
# Check status
docker compose ps

# Test connectivity
docker exec scada-web curl http://scada-server:10000
```

### Out of Disk Space

```bash
docker system prune -a
docker volume prune
```

## Image Sizes

- Total stack: ~840 MB (agent 180 MB, server 200 MB, comm 220 MB, web 240 MB)

## Security

- Non-root execution (`scada:scada` user)
- Isolated Docker network
- No hardcoded secrets
- Read-only volume mounts supported

## Support

- [Documentation](https://rapidscada.net/docs/)
- [Forum](https://forum.rapidscada.org/)
- [GitHub Issues](https://github.com/RapidScada/scada-v6/issues)

## License

Apache 2.0 - See LICENSE.txt in project root
