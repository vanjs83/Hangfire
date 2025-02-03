pipeline {
    agent any

    environment {
        SERVICE_NAME = "MyWindowsService"   // Change to your service name
        DEPLOY_PATH = "C:\\Services\\MyWindowsService"  // Path where service runs
    }

    stages {
        stage('Checkout Code') {
            steps {
                git branch: 'main', 
                    credentialsId: 'github-credentials-id', 
                    url: 'https://github.com/your-username/your-dotnet-repo.git'
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
                    sc stop ${SERVICE_NAME} || echo Service not running
                    """
                }
            }
        }

        stage('Deploy Service') {
            steps {
                script {
                    bat """
                    echo Deploying Windows Service...
                    if not exist ${DEPLOY_PATH} mkdir ${DEPLOY_PATH}
                    xcopy /E /Y publish_output\\* ${DEPLOY_PATH}\\
                    """
                }
            }
        }

        stage('Start Windows Service') {
            steps {
                script {
                    bat """
                    echo Starting Windows Service...
                    sc start ${SERVICE_NAME}
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
