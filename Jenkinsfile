pipeline {
    agent any
    environment {
        SERVICE_NAME = "HangfireService"   // Change to your service name
        DEPLOY_PATH = "C:\\Services\\Hangfire"  // Path where service runs
        COMMIT_SHA = ""
    }

    stages {
        stage('Checkout Code') {
            steps {
                git branch: 'main', 
                    credentialsId: '5220c52c-827d-48be-af22-39373106b6cd', 
                    url: 'https://github.com/vanjs83/Hangfire.git'
            }
        }

          stage('Get Commit SHA') {
            steps {
                script {
                    def commitSha = sh(script: "git rev-parse --short=7 HEAD", returnStdout: true).trim()
                    // Use withEnv to ensure variable persists
                   echo withEnv(["COMMIT_SHA=${commitSha}"]) {
                    echo "✅ Stored COMMIT_SHA: ${env.COMMIT_SHA}"
                }
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
                bat 'dotnet publish -c Release -o publish_output'
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
        
         stage('Install Windows Service') {
            steps {
                script {
                    bat """
                    echo Installing Windows Service...
                    sc query ${SERVICE_NAME} >nul 2>&1
                    if errorlevel 1 (
                        echo Service does not exist. Installing service...
                        sc create ${SERVICE_NAME} binPath= "${DEPLOY_PATH}\\Hangfire.exe" start= auto
                    ) else (
                        echo Service already installed.
                    )
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
            echo '✅ Build and Deployment Successful from commit SHA'

        }
        failure {
            echo '❌ Build Failed. Check logs.'
        }
    }
}
