/* groovylint-disable-next-line CompileStatic */
pipeline {
  agent any
  environment {
        NVM_DIR = '/var/lib/jenkins/.nvm'
        GIT_SSH_COMMAND = "ssh -vvv -i ${env.JENKINS_HOME}/.ssh/id_ed25519"
  }
  stages {
    stage('Pull') {
      steps {
        sh '''
            cd /home/data/crm_yes_location
            git config --global --add safe.directory /home/data/crm_yes_location
            git checkout main
            git reset --hard HEAD
            git pull
        '''
      }
    }
    stage('Build Frontend') {
      steps {
        sh """
            cd /home/data/crm_yes_location/frontend
            . ${NVM_DIR}/nvm.sh
            nvm use --silent 18.19.0
            npm install
            npm run build
        """
      }
    }
    stage('Build and Deploy Backend') {
      steps {
        sh '''
            cd /home/data/crm_yes_location/backend
            # Build l'image Docker
            docker build -t yes-location-api .
            # Arrêter et supprimer l'ancien container s'il existe
            docker stop yes-location-api || true
            docker rm yes-location-api || true
            # Démarrer le nouveau container
            docker run -d --name yes-location-api \
              -p 5000:80 \
              -v /home/data/crm_yes_location/backend/data:/app/data \
              yes-location-api
        '''
      }
    }
  }
  post {
    always {
      cleanWs()
    }
  }
}
