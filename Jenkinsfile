pipeline {
    agent any
    environment {
        dotnet = 'C:\\Program Files\\dotnet\\dotnet.exe'
    }
    stages {
        stage('Build Stage') {
            steps {
                bat 'C:\\Users\\axservices\\AppData\\Local\\Jenkins\\.jenkins\\workspace\\SparepartManagementSystem\\SparepartManagementSystem.sln --configuration Release'
            }
        }
        stage("Release Stage") {
            steps {
                bat 'dotnet build %WORKSPACE%\\SparepartManagementSystem.sln /p:PublishProfile=" %WORKSPACE%\\SparepartManagementSystem\\SparepartManagementSystem.API\\Properties\\PublishProfiles\\FolderProfile.pubxml" /p:Platform="Any CPU" /p:DeployOnBuild=true /m'
                bat '%WORKSPACE%\\SparepartManagementSystem\\SparepartManagementSystem.API\\bin\\Release\\net8.0\\publish\\SparepartManagementSystem.API.deploy.cmd /Y'
            }
        }
    }
}