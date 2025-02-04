pipeline {
    agent any

    environment {
        SERVICE_NAME = "HangfireService"   // Change to your service name
        DEPLOY_PATH = "C:\\Services\\HangfireService"  // Path where service runs
    }

    stages {
        stage('Checkout Code') {
            steps {
                git branch: 'main', 
                    credentialsId: '5220c52c-827d-48be-af22-39373106b6cd', 
                    url: 'https://github.com/vanjs83/your-dotnet-repo.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build Application') {
            steps {
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Run Tests') {
            steps {
                bat 'dotnet test --configuration Release'
            }
        }

        stage('Publish Windows Service') {
            steps {
                bat 'dotnet publish -c Release -o ${DEPLOY_PATH}'
            }
        }

        stage('Stop Windows Service') {
            steps {
                script {
                    bat """
                    echo Stopping Windows Service...
                   echo sc stop ${SERVICE_NAME} || echo Service not running
                    """
                }
            }
        }

        stage('Deploy Service') {
            steps {
                script {
                    bat """
                    echo Deploying Windows Service...
                   echo if not exist ${DEPLOY_PATH} mkdir ${DEPLOY_PATH}
                  echo  xcopy /E /Y publish_output\\* ${DEPLOY_PATH}\\
                    """
                }
            }
        }

        stage('Start Windows Service') {
            steps {
                script {
                    bat """
                  echo  echo Starting Windows Service...
                   echo sc start ${SERVICE_NAME}
                    """
                }
            }
        }
    }

    post {
        success {
            echo '✅ Build and Deployment Successful!'
        }
        failure {
            echo '❌ Build Failed. Check logs.'
        }
    }
}
