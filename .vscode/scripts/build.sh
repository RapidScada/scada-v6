#!/bin/bash

red=`tput setaf 1`
green=`tput setaf 2`
reset=`tput sgr0`
execute() { echo "${green}\$ $@ ${reset}" ; "$@" ; }

task="build"
config="Release"
options="/property:GenerateFullPaths=true /consoleloggerparameters:Summary"

while getopts ":t:c:" opt; do
  case $opt in
    t) task="$OPTARG"
    ;;
    c) config="$OPTARG"
    ;;
    \?) echo "Invalid option -$OPTARG" >&2
    ;;
  esac
done

execute dotnet $task ./ScadaCommon/ScadaCommon.sln -c $config $options
execute dotnet $task ./ScadaAgent/ScadaAgent/ScadaAgent.sln -c $config $options
execute dotnet $task ./ScadaComm/ScadaComm/ScadaComm.sln -c $config $options
execute dotnet $task ./ScadaWeb/ScadaWeb/ScadaWeb.sln -c $config $options
execute dotnet $task ./ScadaAdmin/ScadaAdmin/ScadaAdmin.sln -c $config $options
execute dotnet $task ./ScadaReport/ScadaReport.sln -c $config $options
execute dotnet $task ./ScadaComm/OpenDrivers/OpenDrivers.sln -c $config $options
execute dotnet $task ./ScadaComm/OpenDrivers2/OpenDrivers2.sln -c $config $options
execute dotnet $task ./ScadaServer/OpenModules/OpenModules.sln -c $config $options
execute dotnet $task ./ScadaWeb/OpenPlugins/OpenPlugins.sln -c $config $options
execute dotnet $task ./ScadaAdmin/OpenExtensions/OpenExtensions.sln -c $config $options
