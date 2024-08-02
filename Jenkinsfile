pipeline {
    agent any
    environment {
        dotnet = 'C:\\Program Files\\dotnet\\dotnet.exe'
    }
    stages {
        stage('Build Stage') {
            steps {
                bat '%WORKSPACE%\\SparepartManagementSystem.sln --configuration Release'
            }
        }
        stage("Release Stage") {
            steps {
                bat 'dotnet build %WORKSPACE%\\SparepartManagementSystem.sln /p:PublishProfile="%WORKSPACE%\\SparepartManagementSystem.API\\Properties\\PublishProfiles\\FolderProfile.pubxml" /p:Platform="Any CPU" /p:DeployOnBuild=true /m'
                bat '%WORKSPACE%\\SparepartManagementSystem.API\\bin\\Release\\net8.0\\publish\\SparepartManagementSystem.API.deploy.cmd /Y'
            }
        }
        stage('Migrate Database') {
            steps {
                bat 'dotnet fm rollback --profile Development -p MySql5 -c "Server=localhost;Database=sparepart_management_system;Uid=root;Pwd=Gmk@Colatta!;AllowLoadLocalInfile=true;" -a %WORKSPACE%\\SparepartManagementSystem.Repository.Migration\\bin\\Release\\net8.0\\SparepartManagementSystem.Repository.Migration.dll all'
                bat 'dotnet fm migrate --profile Development -p MySql5 -c "Server=localhost;Database=sparepart_management_system;Uid=root;Pwd=Gmk@Colatta!;AllowLoadLocalInfile=true;" -a %WORKSPACE%\\SparepartManagementSystem.Repository.Migration\\bin\\Release\\net8.0\\SparepartManagementSystem.Repository.Migration.dll'
            }
        }
    }
}