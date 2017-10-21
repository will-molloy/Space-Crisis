#! /bin/sh

project="UnityProject"

echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity 
  -batchmode 
  -nographics 
  -silent-crashes 
  -logFile $(pwd)/unity.log 
  -projectPath $(pwd) 
  -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" 
  -quit

echo 'Logs from build'
cat $(pwd)/unity.log

echo 'Attempting to zip builds'
zip -r $(pwd)/Build/windows.zip $(pwd)/Build/windows/