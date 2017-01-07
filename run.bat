cd TestHarness\bin\Debug
start .\TestHarness.exe 8085 8083
cd ../../../Repository\bin\Debug
start .\Repository.exe 8083
cd ../../../ClientGUI\bin\Debug
start .\ClientGUI.exe 8084 8085 8083
cd ../../../TestExecutive\bin\Debug
start .\TestExecutive.exe 8085 8085 8083
cd ../../../
@pause