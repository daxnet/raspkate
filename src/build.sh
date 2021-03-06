#! /bin/bash
PARAM=''
if [ "$1" == 'Debug' ]
  then
    PARAM='/property:Configuration=Debug;TargetFrameworkVersion=v4.5 Raspkate.sln'
elif [ "$1" == 'All' ]
  then
    PARAM='/property:Configuration=All;TargetFrameworkVersion=v4.5 Raspkate.sln'
elif [ "$1" == 'Minimal' ]
  then
    PARAM='/property:Configuration=Minimal;TargetFrameworkVersion=v4.5 Raspkate.sln'
else
  printf "\n"
  printf "Raspkate Command-Line Build Tool v1.0\n\n"
  printf "Usage:\n"
  printf "    build.sh Debug\n"
  printf "        Builds the Raspkate with Debug configuration.\n\n"
  printf "    build.sh Release\n"
  printf "        Builds the Raspkate with Release configuration.\n\n"
  exit $?
fi

xbuild $PARAM
