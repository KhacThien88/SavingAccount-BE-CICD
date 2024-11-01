pipeline {
  environment {
    dockerimagename = "ktei8htop15122004/savingaccount_be-sa-api"
    dockerImage = ""
    DOCKERHUB_CREDENTIALS = credentials('dockerhub')
  }

  agent {
    kubernetes {
      yaml '''
      apiVersion: v1
      kind: Pod
      spec:
        serviceAccountName: jenkins-admin
        dnsConfig:
          nameservers:
            - 8.8.8.8
        containers:
        - name: docker
          image: docker:latest
          imagePullSecrets:
            - name: regcred
          command:
            - cat
          tty: true
          securityContext:
            privileged: true
          volumeMounts:
            - mountPath: /var/run/docker.sock
              name: docker-sock
        - name: kubectl
          image: bitnami/kubectl:latest
          imagePullSecrets:
            - name: regcred
          command:
            - cat
          securityContext:
            runAsUser: 0
          tty: true
        volumes:
          - name: docker-sock
            hostPath:
              path: /var/run/docker.sock
      '''
    }
  }

  stages {
    stage('Unit Test') {
      when {
        expression {
          return env.BRANCH_NAME != 'master';
        }
      }
      steps {
        sh 'echo Unit Test'
      }
    }

    stage('Build image') {
      steps {
        container('docker') {
          script {
            sh 'docker build --network=host -t ktei8htop15122004/savingaccount_be-sa-api .'
          }
        }
      }
    }

    stage('Pushing Image') {
      steps {
        container('docker') {
          script {
            sh 'echo $DOCKERHUB_CREDENTIALS_PSW | docker login -u $DOCKERHUB_CREDENTIALS_USR --password-stdin'
            sh 'docker tag ktei8htop15122004/savingaccount_be-sa-api ktei8htop15122004/savingaccount_be-sa-api'
            sh 'docker push ktei8htop15122004/savingaccount_be-sa-api:latest'
          }
        }
      }
    }
//     stage('Create SQL Deployment YAML') {
//   steps {
//     writeFile file: '/home/jenkins/agent/workspace/SavingAccountBE_main/deployment-sql.yaml', text: '''apiVersion: apps/v1
// kind: Deployment
// metadata:
//   name: sqlserver
//   labels:
//     app: sqlserver
// spec:
//   replicas: 1
//   selector:
//     matchLabels:
//       app: sqlserver
//   template:
//     metadata:
//       labels:
//         app: sqlserver
//     spec:
//       containers:
//       - name: sqlserver
//         image: mcr.microsoft.com/mssql/server:2019-latest
//         ports:
//         - containerPort: 1433
//         env:
//         - name: MSSQL_SA_PASSWORD
//           value: "1236fG543$"
//         - name: ACCEPT_EULA
//           value: "Y"
//         volumeMounts:
//         - name: sql-data
//           mountPath: /var/opt/mssql
//       volumes:
//       - name: sql-data
//         persistentVolumeClaim:
//           claimName: sql-data-pvc
// '''
//   }
// }

    stage('Create Deployment YAML for BE') {
  steps {
    writeFile file: '/home/jenkins/agent/workspace/SavingAccountBE_main/deployment-be.yaml', text: '''apiVersion: apps/v1
kind: Deployment
metadata:
  name: be-app-deployment
  labels:
    app: be-app
spec:
  replicas: 2
  selector:
    matchLabels:
      app: be-app
  template:
    metadata:
      labels:
        app: be-app
        app.kubernetes.io/name: be-app
    spec:
      containers:
      - name: savingaccountbe
        image: ktei8htop15122004/savingaccount_be-sa-api:latest
        ports:
        - containerPort: 3334
        resources:
          requests:
            cpu: 100m
            memory: 100Mi
          limits:
            cpu: 200m
            memory: 200Mi
        env:
        - name: ASPCORE_URLS
          value: http://+:3334
        - name: ASPNETCORE_ENVIRONMENT
          value: "Development"
        - name: ConnectionStrings__UsersDatabase
          value: "workstation id=SavingAccountDB.mssql.somee.com;packet size=4096;user id=KTeightop1512_SQLLogin_1;pwd=7n1cxj9rw9;data source=SavingAccountDB.mssql.somee.com;persist security info=False;initial catalog=SavingAccountDB;TrustServerCertificate=True"
'''
  }
}

// stage('Create SQL Service YAML') {
//   steps {
//     writeFile file: '/home/jenkins/agent/workspace/SavingAccountBE_main/service-sql.yaml', text: '''apiVersion: v1
// kind: Service
// metadata:
//   name: sqlserver-svc
// spec:
//   selector:
//     app: sqlserver
//   ports:
//     - protocol: TCP
//       port: 1433
//       targetPort: 1433
// '''
//   }
// }

    stage('Create Service YAML') {
    steps {
        writeFile file: '/home/jenkins/agent/workspace/SavingAccountBE_main/service-be.yaml', text: '''apiVersion: v1
kind: Service
metadata:
  name: be-app-svc
spec:
  type: NodePort
  selector:
    app: be-app
  ports:
    - name: http
      protocol: TCP 
      port: 81
      targetPort: 3334
      nodePort: 32101'''
    }
}

    stage('Deploying App to Kubernetes') {
      steps {
        container('kubectl') {
          withCredentials([file(credentialsId: 'kube-config-admin', variable: 'TMPKUBECONFIG')]) {
            sh "cat \$TMPKUBECONFIG"
            sh "cp \$TMPKUBECONFIG ~/.kube/config"
            sh "ls -l \$TMPKUBECONFIG"
            sh "pwd"
            sh "kubectl apply -f deployment-be.yaml"
            sh "kubectl apply -f service-be.yaml"
          }
        }
      }
    }
  }
}