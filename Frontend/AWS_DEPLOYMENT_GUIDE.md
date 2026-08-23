# AWS EC2 Free Tier Deployment Guide for CommunicaAI

## Table of Contents
1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [AWS Services Required](#aws-services-required)
4. [Step-by-Step Deployment](#step-by-step-deployment)
5. [Post-Deployment Configuration](#post-deployment-configuration)
6. [Troubleshooting](#troubleshooting)
7. [Cost Management](#cost-management)

---

## Overview

### What You'll Deploy
This guide deploys the **CommunicaAI** application on AWS using **EC2 Free Tier**. The architecture includes:
- 1 EC2 t2.micro instance (Backend + Frontend + Database on same server)
- PostgreSQL database (installed on EC2)
- Nginx as reverse proxy
- Domain/IP for public access

### Why AWS EC2?
- **Cost**: Free for 12 months (750 hours/month)
- **Control**: Full server access for customization
- **Learning**: Industry-standard cloud platform
- **Flexibility**: Can scale when needed

### Architecture Diagram
```
Internet → AWS EC2 Instance (t2.micro)
                ├── Nginx (Port 80/443) → Frontend (Angular)
                ├── .NET Backend (Port 5000)
                └── PostgreSQL (Port 5432)
```

---

## Prerequisites

### 1. AWS Account Setup
**WHY**: You need an AWS account to create and manage cloud resources.

**STEPS**:
1. Go to https://aws.amazon.com/
2. Click "Create an AWS Account"
3. Provide email, password, and account name
4. Enter payment details (required even for free tier - won't be charged unless you exceed limits)
5. Verify identity with phone number
6. Choose "Basic Support - Free" plan
7. Complete account activation

**VERIFICATION**: You should receive a confirmation email and be able to login to AWS Console at https://console.aws.amazon.com/

### 2. Local Requirements
- Git installed on your machine
- GitHub account with CommunicaAI repository
- Gemini API key from https://aistudio.google.com/
- Cloudinary account credentials from https://cloudinary.com/

---

## AWS Services Required

### 1. EC2 (Elastic Compute Cloud)
**PURPOSE**: Virtual server to run your application
**WHY NEEDED**: Hosts backend, frontend, and database
**FREE TIER**: 750 hours/month of t2.micro instance (enough for 24/7 operation)
**COST IF EXCEEDED**: ~$0.0116/hour (~$8.50/month)

### 2. Elastic IP (Optional but Recommended)
**PURPOSE**: Static public IP address
**WHY NEEDED**: Without it, IP changes every time you stop/start EC2
**FREE TIER**: 1 Elastic IP free when attached to running instance
**COST IF EXCEEDED**: $0.005/hour if not attached to running instance

### 3. Security Groups
**PURPOSE**: Firewall rules for EC2 instance
**WHY NEEDED**: Controls which ports are accessible from internet
**FREE TIER**: Always free
**COST**: None

### 4. EBS (Elastic Block Store)
**PURPOSE**: Hard drive storage for EC2 instance
**WHY NEEDED**: Stores application files, database, logs
**FREE TIER**: 30 GB of General Purpose (SSD) storage
**COST IF EXCEEDED**: $0.10/GB/month

---

## Step-by-Step Deployment

### PHASE 1: Launch EC2 Instance

#### Step 1.1: Access EC2 Dashboard
```
ACTION: Navigate to EC2 service in AWS Console
WHY: This is where you create and manage virtual servers

DETAILED STEPS:
1. Login to AWS Console at https://console.aws.amazon.com/
2. In the search bar at top, type "EC2"
3. Click "EC2" from the dropdown
4. You should see the EC2 Dashboard
```

#### Step 1.2: Launch Instance
```
ACTION: Create a new EC2 instance
WHY: This creates the virtual server for your application

DETAILED STEPS:
1. Click orange "Launch Instance" button (top right)
2. You'll see "Launch an instance" page
```

#### Step 1.3: Configure Instance Details

**Name and Tags**
```
FIELD: Name
VALUE: CommunicaAI-Server
WHY: Helps identify your instance in the AWS console

ACTION: Enter "CommunicaAI-Server" in the Name field
```

**Application and OS Images (Amazon Machine Image)**
```
FIELD: AMI
VALUE: Ubuntu Server 22.04 LTS (HVM), SSD Volume Type
WHY: Ubuntu is stable, well-documented, and free tier eligible

DETAILED STEPS:
1. Under "Application and OS Images" section
2. Click "Quick Start" tab (should be selected by default)
3. Click "Ubuntu" option
4. Select "Ubuntu Server 22.04 LTS (HVM), SSD Volume Type"
5. Ensure "64-bit (x86)" architecture is selected
6. Verify "Free tier eligible" tag is visible
```

**Instance Type**
```
FIELD: Instance type
VALUE: t2.micro
WHY: Only instance type eligible for free tier (1 vCPU, 1 GB RAM)

DETAILED STEPS:
1. Under "Instance type" section
2. Click dropdown if not already showing t2.micro
3. Select "t2.micro" (should show "Free tier eligible")
4. Do NOT select t2.small, t2.medium, or any other type - they are NOT free
```

**Key Pair (Login)**
```
FIELD: Key pair
VALUE: Create new key pair named "communicaai-key"
WHY: SSH key required to securely connect to your server

DETAILED STEPS:
1. Under "Key pair (login)" section
2. Click "Create new key pair" link
3. A modal dialog will appear:
   - Key pair name: communicaai-key
   - Key pair type: RSA
   - Private key file format: .pem (for Mac/Linux) or .ppk (for Windows with PuTTY)
4. Click "Create key pair" button
5. A file named "communicaai-key.pem" (or .ppk) will download
6. IMPORTANT: Save this file securely - you cannot download it again
7. Move the file to a safe location (e.g., ~/.ssh/ on Mac/Linux or C:\Users\YourName\.ssh\ on Windows)
```

**Network Settings**
```
PURPOSE: Configure firewall rules
WHY: Allow HTTP, HTTPS, and SSH access to your server

DETAILED STEPS:
1. Under "Network settings" section
2. Click "Edit" button (top right of the section)
3. Configure the following:

   VPC: Leave default (your default VPC)
   Subnet: Leave as "No preference"
   Auto-assign public IP: Enable (should be enabled by default)
   
   Firewall (security groups): Create security group
   Security group name: communicaai-sg
   Description: Security group for CommunicaAI application

4. Add Security Group Rules (click "Add security group rule" for each):

   Rule 1 - SSH (for you to connect):
   - Type: SSH
   - Protocol: TCP
   - Port: 22
   - Source type: My IP
   - Why: Allows you to connect to server via SSH
   
   Rule 2 - HTTP (for website visitors):
   - Type: HTTP
   - Protocol: TCP
   - Port: 80
   - Source type: Anywhere (0.0.0.0/0)
   - Why: Allows anyone to access your website
   
   Rule 3 - HTTPS (for secure website):
   - Type: HTTPS
   - Protocol: TCP
   - Port: 443
   - Source type: Anywhere (0.0.0.0/0)
   - Why: Allows secure HTTPS connections
   
   Rule 4 - Custom TCP (for backend API):
   - Type: Custom TCP
   - Protocol: TCP
   - Port: 5000
   - Source type: Anywhere (0.0.0.0/0)
   - Why: Allows access to .NET backend API
```

**Configure Storage**
```
FIELD: Storage
VALUE: 30 GB gp3 (General Purpose SSD)
WHY: Maximum free tier allowance for storage

DETAILED STEPS:
1. Under "Configure storage" section
2. Default should show 8 GB - change this to 30 GB
3. Volume type: gp3 (General Purpose SSD) - Free tier eligible
4. Do not add additional volumes
5. Encryption: Not needed (keep unencrypted to save resources)
6. Delete on termination: Check this (storage deleted when instance deleted)
```

**Advanced Details**
```
ACTION: Leave all advanced settings as default
WHY: Default settings are sufficient for our needs

SKIP THIS SECTION - No changes needed
```

#### Step 1.4: Launch Instance
```
ACTION: Review and launch the instance

DETAILED STEPS:
1. Review all settings in the "Summary" panel on right side
2. Verify:
   - Name: CommunicaAI-Server
   - AMI: Ubuntu Server 22.04 LTS
   - Instance type: t2.micro (Free tier eligible)
   - Key pair: communicaai-key
   - Security group: communicaai-sg with 4 rules
   - Storage: 30 GB gp3
3. Click orange "Launch instance" button (bottom right)
4. You'll see "Successfully initiated launch of instance"
5. Click "View all instances" link
```

#### Step 1.5: Wait for Instance to Start
```
ACTION: Monitor instance status
WHY: Instance needs to initialize before you can connect

DETAILED STEPS:
1. You should now see your instance in the instances list
2. Initially, "Instance state" shows "Pending" (orange)
3. Wait 1-2 minutes - status will change to "Running" (green)
4. "Status check" will show "Initializing" then "2/2 checks passed"
5. Once running and checks passed, you're ready to connect
```

#### Step 1.6: Note Important Details
```
ACTION: Copy instance connection details
WHY: You'll need these to connect via SSH

DETAILED STEPS:
1. Select your instance (click checkbox)
2. In the details panel below, note:
   - Instance ID: i-xxxxxxxxxxxxxxxxx
   - Public IPv4 address: XX.XX.XX.XX (e.g., 54.123.45.67)
   - Public IPv4 DNS: ec2-XX-XX-XX-XX.compute-1.amazonaws.com
3. Save these somewhere - you'll need them shortly
```

---

### PHASE 2: Connect to EC2 Instance

#### Step 2.1: Connect via SSH

**For Mac/Linux Users:**
```bash
# WHY: Set correct permissions on key file (required by SSH)
# WHAT IT DOES: Makes key file readable only by you
chmod 400 ~/path/to/communicaai-key.pem

# WHY: Connect to your EC2 instance
# WHAT IT DOES: Opens secure shell connection to your server
# REPLACE: XX.XX.XX.XX with your instance's Public IPv4 address
ssh -i ~/path/to/communicaai-key.pem ubuntu@XX.XX.XX.XX

# EXPECTED OUTPUT: You'll see a prompt asking "Are you sure you want to continue connecting?"
# TYPE: yes
# EXPECTED RESULT: You'll see Ubuntu welcome message and prompt: ubuntu@ip-xxx-xxx-xxx-xxx:~$
```

**For Windows Users (using PuTTY):**
```
STEPS:
1. Download PuTTY from https://www.putty.org/
2. Open PuTTYgen (comes with PuTTY)
3. Load your .ppk file (or convert .pem to .ppk if needed)
4. Open PuTTY
5. Host Name: ubuntu@XX.XX.XX.XX (your instance IP)
6. Port: 22
7. Connection type: SSH
8. Under Connection > SSH > Auth > Credentials: Browse and select your .ppk file
9. Click "Open"
10. Accept security alert
11. You should now be connected to your server
```

#### Step 2.2: Verify Connection
```bash
# WHY: Confirm you're connected to the correct server
# WHAT IT DOES: Shows system information
uname -a

# EXPECTED OUTPUT: Shows "Ubuntu" and "x86_64" in the output
# Example: Linux ip-172-31-xx-xx 5.15.0-1031-aws #35-Ubuntu SMP...

# WHY: Check available disk space
# WHAT IT DOES: Shows storage usage
df -h

# EXPECTED OUTPUT: Should show about 30GB total on /dev/root
```

---

### PHASE 3: Install Required Software

#### Step 3.1: Update System Packages
```bash
# WHY: Get latest security updates and package lists
# WHAT IT DOES: Downloads updated package information from repositories
# HOW LONG: 1-2 minutes
sudo apt update

# EXPECTED OUTPUT: Multiple lines showing "Hit" or "Get" for package repositories
# WAIT FOR: Command prompt to return

# WHY: Install available updates
# WHAT IT DOES: Upgrades installed packages to latest versions
# HOW LONG: 3-5 minutes
sudo apt upgrade -y

# EXPECTED OUTPUT: Shows packages being upgraded
# -y FLAG: Automatically answers "yes" to prompts
# WAIT FOR: "Processing triggers" messages and command prompt
```

#### Step 3.2: Install .NET 8.0 SDK
```bash
# WHY: .NET SDK is required to run the backend application
# WHAT IT DOES: Adds Microsoft package repository to system

# Step 1: Download Microsoft package signing key
# WHY: Verifies packages are from Microsoft
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# EXPECTED OUTPUT: Shows download progress, saves file
# WAIT FOR: "saved" message

# Step 2: Install the repository configuration
# WHY: Tells Ubuntu where to find Microsoft packages
sudo dpkg -i packages-microsoft-prod.deb

# EXPECTED OUTPUT: "Selecting previously unselected package..." message

# Step 3: Remove the downloaded file
# WHY: Clean up - no longer needed
rm packages-microsoft-prod.deb

# Step 4: Update package list again
# WHY: Include newly added Microsoft repository
sudo apt update

# Step 5: Install .NET SDK 8.0
# WHY: Required to build and run .NET applications
# HOW LONG: 2-3 minutes
sudo apt install -y dotnet-sdk-8.0

# EXPECTED OUTPUT: Shows installation of dotnet packages
# WAIT FOR: Command prompt

# Step 6: Verify installation
# WHY: Confirm .NET installed correctly
dotnet --version

# EXPECTED OUTPUT: Should show version like "8.0.xxx"
# IF ERROR: Run installation steps again
```

#### Step 3.3: Install PostgreSQL Database
```bash
# WHY: PostgreSQL is the database for storing application data
# WHAT IT DOES: Installs PostgreSQL server and client tools
# HOW LONG: 1-2 minutes
sudo apt install -y postgresql postgresql-contrib

# EXPECTED OUTPUT: Shows installation of postgresql packages
# WAIT FOR: Command prompt

# WHY: Verify PostgreSQL is running
# WHAT IT DOES: Shows service status
sudo systemctl status postgresql

# EXPECTED OUTPUT: Should show "active (running)" in green
# PRESS: q to exit status view
# IF NOT RUNNING: sudo systemctl start postgresql

# WHY: Enable PostgreSQL to start on boot
# WHAT IT DOES: Ensures database starts when server restarts
sudo systemctl enable postgresql

# EXPECTED OUTPUT: May show "Synchronizing state of postgresql.service" or already enabled
```

#### Step 3.4: Install Node.js and npm
```bash
# WHY: Node.js required to build Angular frontend
# WHAT IT DOES: Installs Node.js 20.x LTS version

# Step 1: Download Node.js setup script
# WHY: Adds NodeSource repository for latest Node.js
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -

# EXPECTED OUTPUT: Shows script execution, repository addition
# WAIT FOR: Command prompt

# Step 2: Install Node.js and npm
# WHY: Installs both Node.js runtime and npm package manager
# HOW LONG: 1-2 minutes
sudo apt install -y nodejs

# EXPECTED OUTPUT: Shows installation progress
# WAIT FOR: Command prompt

# Step 3: Verify installation
# WHY: Confirm versions are correct
node --version
npm --version

# EXPECTED OUTPUT:
# node: v20.x.x
# npm: 10.x.x
# IF ERROR: Repeat installation steps
```

#### Step 3.5: Install Nginx Web Server
```bash
# WHY: Nginx serves the frontend and proxies API requests
# WHAT IT DOES: Installs Nginx reverse proxy server
# HOW LONG: 30 seconds
sudo apt install -y nginx

# EXPECTED OUTPUT: Shows nginx package installation
# WAIT FOR: Command prompt

# WHY: Start Nginx service
# WHAT IT DOES: Starts the web server
sudo systemctl start nginx

# WHY: Enable Nginx to start on boot
# WHAT IT DOES: Ensures web server starts after reboot
sudo systemctl enable nginx

# EXPECTED OUTPUT: May show synchronizing message

# WHY: Verify Nginx is running
# WHAT IT DOES: Shows service status
sudo systemctl status nginx

# EXPECTED OUTPUT: Should show "active (running)" in green
# PRESS: q to exit

# WHY: Test Nginx is accessible
# WHAT IT DOES: Opens Nginx welcome page in browser
# ACTION: Open browser and go to: http://XX.XX.XX.XX (your instance IP)
# EXPECTED: Should see "Welcome to nginx!" page
# IF NOT WORKING: Check security group allows port 80
```

#### Step 3.6: Install Git
```bash
# WHY: Git required to clone your application code
# WHAT IT DOES: Installs Git version control system
# HOW LONG: 30 seconds
sudo apt install -y git

# EXPECTED OUTPUT: Shows git package installation
# WAIT FOR: Command prompt

# WHY: Verify installation
# WHAT IT DOES: Shows Git version
git --version

# EXPECTED OUTPUT: git version 2.34.x or similar
```

---

### PHASE 4: Configure PostgreSQL Database

#### Step 4.1: Create Database and User
```bash
# WHY: Switch to postgres system user
# WHAT IT DOES: Changes to user that has database admin rights
sudo -i -u postgres

# EXPECTED OUTPUT: Prompt changes to "postgres@ip-xxx-xxx-xxx-xxx:~$"

# WHY: Open PostgreSQL command-line interface
# WHAT IT DOES: Starts interactive SQL session
psql

# EXPECTED OUTPUT: Shows PostgreSQL version and prompt "postgres=#"

# WHY: Create database for application
# WHAT IT DOES: Creates empty database named CommunicaAIDB
# PASTE THIS EXACT COMMAND:
CREATE DATABASE "CommunicaAIDB";

# EXPECTED OUTPUT: CREATE DATABASE
# NOTE: Database name is case-sensitive with quotes

# WHY: Create database user with password
# WHAT IT DOES: Creates user that application will use to connect
# REPLACE: 'YourSecurePassword123!' with a strong password
# PASTE THIS COMMAND (replace password):
CREATE USER communicaai WITH PASSWORD 'YourSecurePassword123!';

# EXPECTED OUTPUT: CREATE ROLE
# IMPORTANT: Remember this password - you'll need it later

# WHY: Grant all privileges to user
# WHAT IT DOES: Allows user to read/write data in database
# PASTE THIS EXACT COMMAND:
GRANT ALL PRIVILEGES ON DATABASE "CommunicaAIDB" TO communicaai;

# EXPECTED OUTPUT: GRANT

# WHY: Grant schema privileges
# WHAT IT DOES: Allows user to create tables and modify schema
# PASTE THESE COMMANDS:
\c CommunicaAIDB

# EXPECTED OUTPUT: You are now connected to database "CommunicaAIDB"

GRANT ALL ON SCHEMA public TO communicaai;

# EXPECTED OUTPUT: GRANT

# WHY: Exit psql
# WHAT IT DOES: Returns to postgres user shell
\q

# EXPECTED OUTPUT: Returns to postgres@... prompt

# WHY: Exit postgres user
# WHAT IT DOES: Returns to ubuntu user
exit

# EXPECTED OUTPUT: Returns to ubuntu@... prompt
```

#### Step 4.2: Configure PostgreSQL for Local Access
```bash
# WHY: PostgreSQL by default only allows local connections
# WHAT IT DOES: Already configured correctly by default for our setup
# NO ACTION NEEDED: Database will be accessed locally from same server

# WHY: Verify database configuration
# WHAT IT DOES: Tests connection with new credentials
# REPLACE: YourSecurePassword123! with your actual password
PGPASSWORD='YourSecurePassword123!' psql -h localhost -U communicaai -d CommunicaAIDB -c "SELECT version();"

# EXPECTED OUTPUT: Shows PostgreSQL version information
# IF ERROR: Check username and password are correct
```

---

### PHASE 5: Deploy Backend Application

#### Step 5.1: Clone Repository
```bash
# WHY: Navigate to web directory
# WHAT IT DOES: Changes to directory where we'll store application
cd /var/www

# WHY: Create directory if it doesn't exist
# WHAT IT DOES: Ensures directory exists with correct permissions
sudo mkdir -p /var/www

# WHY: Change ownership to ubuntu user
# WHAT IT DOES: Allows ubuntu user to write files here without sudo
sudo chown -R ubuntu:ubuntu /var/www

# WHY: Clone your GitHub repository
# WHAT IT DOES: Downloads application code from GitHub
# REPLACE: VigneshGazula with your actual GitHub username if different
git clone https://github.com/VigneshGazula/CommunicaAI.git

# EXPECTED OUTPUT: Shows "Cloning into 'CommunicaAI'..."
# HOW LONG: 30 seconds - 1 minute
# WAIT FOR: "Resolving deltas: 100%, done."

# WHY: Navigate to backend directory
# WHAT IT DOES: Changes to backend code directory
cd CommunicaAI/CommunicaAI
```

#### Step 5.2: Configure Backend Settings
```bash
# WHY: Create production configuration file
# WHAT IT DOES: Sets up environment-specific settings
# COMMAND: Open nano text editor to create config file
sudo nano appsettings.Production.json

# WHAT TO DO IN NANO EDITOR:
# 1. Copy and paste the JSON below
# 2. Replace all placeholder values (marked with YOUR_...)
# 3. Press Ctrl+X to exit
# 4. Press Y to save
# 5. Press Enter to confirm filename
```

**PASTE THIS JSON INTO NANO (after replacing values):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=CommunicaAIDB;Username=communicaai;Password=YourSecurePassword123!"
  },
  "Jwt": {
    "Issuer": "CommunicaAI",
    "Audience": "CommunicaAIUsers",
    "Key": "your-very-long-secret-key-at-least-32-characters-long"
  },
  "CloudinarySettings": {
    "CloudName": "YOUR_CLOUDINARY_CLOUD_NAME",
    "ApiKey": "YOUR_CLOUDINARY_API_KEY",
    "ApiSecret": "YOUR_CLOUDINARY_API_SECRET"
  },
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY",
    "Model": "gemini-2.0-flash-exp"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**EXPLANATION OF EACH FIELD:**

```
ConnectionStrings.DefaultConnection:
- WHY: Tells backend how to connect to database
- Host=localhost: Database is on same server
- Port=5432: Default PostgreSQL port
- Database=CommunicaAIDB: Name we created earlier
- Username=communicaai: User we created earlier
- Password: The password you set in Step 4.1
- REPLACE: YourSecurePassword123! with your actual password

Jwt.Key:
- WHY: Used to encrypt/decrypt authentication tokens
- MUST BE: At least 32 characters long
- EXAMPLE: "MyApp$SuperSecure#JWT@Key!2024ForProd"
- REPLACE: Generate a random 32+ character string

CloudinarySettings:
- WHY: Stores audio files uploaded by users
- WHERE TO GET:
  1. Login to https://cloudinary.com/
  2. Go to Dashboard
  3. Copy Cloud Name, API Key, API Secret
- REPLACE: All three YOUR_CLOUDINARY_... values

Gemini.ApiKey:
- WHY: Used for AI transcription and evaluation
- WHERE TO GET:
  1. Go to https://aistudio.google.com/app/apikey
  2. Click "Create API Key"
  3. Copy the key (starts with "AIza...")
- REPLACE: YOUR_GEMINI_API_KEY with actual key
```

#### Step 5.3: Apply Database Migrations
```bash
# WHY: Create database tables and schema
# WHAT IT DOES: Runs Entity Framework migrations to set up database structure
# HOW LONG: 10-20 seconds
dotnet ef database update

# EXPECTED OUTPUT: Shows migration names being applied
# Example:
# Applying migration '20240101000000_InitialCreate'.
# Applying migration '20240102000000_AddInterviewTables'.
# ...
# Done.

# IF ERROR "dotnet ef not found":
# SOLUTION: Install EF Core tools
dotnet tool install --global dotnet-ef
export PATH="$PATH:$HOME/.dotnet/tools"
dotnet ef database update

# IF ERROR "No migrations found":
# SOLUTION: Check you're in correct directory (CommunicaAI/CommunicaAI)
pwd  # Should show /var/www/CommunicaAI/CommunicaAI
```

#### Step 5.4: Build and Publish Backend
```bash
# WHY: Compile application for production
# WHAT IT DOES: Builds optimized production version
# HOW LONG: 1-2 minutes
dotnet publish -c Release -o /var/www/CommunicaAI/publish

# EXPECTED OUTPUT: Shows build progress
# Example:
# MSBuild version 17.x.x
# Determining projects to restore...
# Restored /var/www/CommunicaAI/CommunicaAI/CommunicaAI.csproj
# ...
# CommunicaAI -> /var/www/CommunicaAI/CommunicaAI/bin/Release/net8.0/CommunicaAI.dll
# CommunicaAI -> /var/www/CommunicaAI/publish/

# WAIT FOR: "Build succeeded" message

# WHY: Verify published files exist
# WHAT IT DOES: Lists published files
ls -la /var/www/CommunicaAI/publish

# EXPECTED OUTPUT: Should show CommunicaAI.dll and other files
```

#### Step 5.5: Create Systemd Service
```bash
# WHY: Run backend as a system service
# WHAT IT DOES: Creates service that starts backend automatically
# COMMAND: Create service file
sudo nano /etc/systemd/system/communicaai-backend.service

# WHAT TO DO: Copy and paste the text below, then save (Ctrl+X, Y, Enter)
```

**PASTE THIS INTO NANO:**
```ini
[Unit]
Description=CommunicaAI Backend API
After=network.target postgresql.service

[Service]
Type=notify
User=ubuntu
WorkingDirectory=/var/www/CommunicaAI/publish
ExecStart=/usr/bin/dotnet /var/www/CommunicaAI/publish/CommunicaAI.dll
Restart=always
RestartSec=10
SyslogIdentifier=communicaai-backend
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

**EXPLANATION OF SERVICE FILE:**

```
[Unit] Section:
- Description: Human-readable name for service
- After: Wait for network and database before starting
- WHY: Ensures dependencies are ready

[Service] Section:
- Type=notify: .NET will notify systemd when ready
- User=ubuntu: Run as ubuntu user (not root for security)
- WorkingDirectory: Where the application files are
- ExecStart: Command to run the backend
- Restart=always: Restart if it crashes
- RestartSec=10: Wait 10 seconds before restarting
- Environment: Sets production mode and disables telemetry

[Install] Section:
- WantedBy: When to start this service (at boot)
```

#### Step 5.6: Start Backend Service
```bash
# WHY: Reload systemd to recognize new service
# WHAT IT DOES: Reads the service file we just created
sudo systemctl daemon-reload

# EXPECTED OUTPUT: No output is normal

# WHY: Enable service to start on boot
# WHAT IT DOES: Service will auto-start after server restart
sudo systemctl enable communicaai-backend

# EXPECTED OUTPUT: Shows "Created symlink..." message

# WHY: Start the backend service now
# WHAT IT DOES: Starts backend API immediately
sudo systemctl start communicaai-backend

# EXPECTED OUTPUT: No output is normal (no errors = success)

# WHY: Check if service started successfully
# WHAT IT DOES: Shows service status
sudo systemctl status communicaai-backend

# EXPECTED OUTPUT: Should show "active (running)" in green
# Example:
# ● communicaai-backend.service - CommunicaAI Backend API
#    Loaded: loaded
#    Active: active (running) since ...
#    ...
#    Now listening on: http://0.0.0.0:5000

# PRESS: q to exit status view

# IF SHOWING "failed" or "inactive":
# ACTION: Check logs for errors
sudo journalctl -u communicaai-backend -n 50

# LOOK FOR: Error messages about configuration or missing files
# COMMON ISSUES:
# - appsettings.Production.json has typos
# - Database connection string is wrong
# - Gemini or Cloudinary keys are invalid
```

#### Step 5.7: Test Backend API
```bash
# WHY: Verify backend is responding
# WHAT IT DOES: Makes HTTP request to health endpoint
curl http://localhost:5000/health

# EXPECTED OUTPUT: JSON response like:
# {"status":"healthy"}

# IF ERROR "Connection refused":
# SOLUTION: Backend is not running, check service status

# IF ERROR "404 Not Found":
# SOLUTION: Check if /health endpoint exists in your Program.cs

# WHY: Test from external IP
# WHAT IT DOES: Tests if API is accessible from internet
# REPLACE: XX.XX.XX.XX with your EC2 public IP
curl http://XX.XX.XX.XX:5000/health

# EXPECTED OUTPUT: Same JSON response
# IF ERROR: Check Security Group allows port 5000
```

---

### PHASE 6: Deploy Frontend Application

#### Step 6.1: Navigate to Frontend Directory
```bash
# WHY: Change to frontend code directory
# WHAT IT DOES: Navigates to Angular application folder
cd /var/www/CommunicaAI/Frontend

# WHY: Verify you're in correct location
# WHAT IT DOES: Shows current directory
pwd

# EXPECTED OUTPUT: /var/www/CommunicaAI/Frontend
```

#### Step 6.2: Install Frontend Dependencies
```bash
# WHY: Install Angular and all required packages
# WHAT IT DOES: Downloads all npm packages listed in package.json
# HOW LONG: 3-5 minutes (downloads ~200MB)
npm install

# EXPECTED OUTPUT: Shows package download progress
# WARNINGS: Some warnings are normal (optional dependencies)
# WAIT FOR: "added xxx packages" message and command prompt

# IF ERROR "npm not found":
# SOLUTION: Node.js not installed properly, go back to Step 3.4

# IF ERROR "EACCES permission denied":
# SOLUTION: Run with sudo (not recommended) or fix npm permissions
sudo chown -R ubuntu:ubuntu ~/.npm
npm install
```

#### Step 6.3: Configure Frontend Environment
```bash
# WHY: Set production API URL
# WHAT IT DOES: Tells frontend where backend API is located
# COMMAND: Edit production environment file
nano src/environments/environment.production.ts

# WHAT TO DO IN NANO:
# 1. Find the apiBaseUrl line
# 2. Replace with your EC2 public IP
# 3. Save and exit (Ctrl+X, Y, Enter)
```

**CHANGE THIS FILE TO:**
```typescript
export const environment = {
  production: true,
  apiBaseUrl: 'http://XX.XX.XX.XX:5000'
};
```

**REPLACE: XX.XX.XX.XX with your actual EC2 public IPv4 address**

**WHY EACH SETTING:**
```
production: true
- WHY: Enables production optimizations
- WHAT IT DOES: Disables debug mode, enables minification

apiBaseUrl: 'http://XX.XX.XX.XX:5000'
- WHY: Frontend needs to know where to send API requests
- WHAT IT DOES: All HTTP calls will go to this address
- IMPORTANT: Use http:// not https:// (we don't have SSL yet)
- IMPORTANT: Don't add trailing slash
```

#### Step 6.4: Build Frontend for Production
```bash
# WHY: Compile Angular app for production deployment
# WHAT IT DOES: Creates optimized, minified production build
# HOW LONG: 2-4 minutes
npm run build

# EXPECTED OUTPUT: Shows Angular compilation progress
# Example:
# ✔ Browser application bundle generation complete.
# ✔ Copying assets complete.
# ✔ Index html generation complete.
# ...
# Build at: 2024-01-01T12:00:00.000Z
# ✔ Compiled successfully.

# WAIT FOR: "Compiled successfully" message

# IF ERROR "Insufficient memory":
# SOLUTION: Build on local machine and copy files
# OR: Increase EC2 instance size temporarily

# IF ERROR "Module not found":
# SOLUTION: npm install might have failed, run it again

# WHY: Verify build output exists
# WHAT IT DOES: Lists built files
ls -la dist/frontend/browser/

# EXPECTED OUTPUT: Should show index.html and many .js files
```

#### Step 6.5: Copy Build to Nginx Directory
```bash
# WHY: Move build files to web server directory
# WHAT IT DOES: Copies compiled frontend to where Nginx serves files

# Step 1: Remove default Nginx content
# WHY: Clear out default welcome page
sudo rm -rf /var/www/html/*

# Step 2: Copy built files to web directory
# WHY: Make frontend accessible via web server
sudo cp -r dist/frontend/browser/* /var/www/html/

# Step 3: Set correct permissions
# WHY: Ensure web server can read files
sudo chown -R www-data:www-data /var/www/html
sudo chmod -R 755 /var/www/html

# Step 4: Verify files copied
# WHY: Confirm all files are in place
ls -la /var/www/html/

# EXPECTED OUTPUT: Should show index.html, main.*.js, etc.
```

---

### PHASE 7: Configure Nginx as Reverse Proxy

#### Step 7.1: Create Nginx Configuration
```bash
# WHY: Configure Nginx to serve frontend and proxy API requests
# WHAT IT DOES: Sets up routing rules
# COMMAND: Create site configuration file
sudo nano /etc/nginx/sites-available/communicaai

# WHAT TO DO: Copy configuration below, replace IP, save
```

**PASTE THIS INTO NANO (after replacing IP):**
```nginx
server {
    listen 80;
    listen [::]:80;
    server_name XX.XX.XX.XX;

    # Frontend - Angular application
    location / {
        root /var/www/html;
        try_files $uri $uri/ /index.html;
    }

    # Backend API - Proxy to .NET
    location /api/ {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # Health check endpoint
    location /health {
        proxy_pass http://localhost:5000;
    }
}
```

**REPLACE: XX.XX.XX.XX with your EC2 public IPv4 address**

**EXPLANATION OF CONFIGURATION:**

```
listen 80:
- WHY: HTTP port for web traffic
- WHAT IT DOES: Nginx listens on port 80 for requests

server_name XX.XX.XX.XX:
- WHY: Identifies which requests to handle
- WHAT IT DOES: Matches requests to your IP
- REPLACE: With your actual EC2 public IP

location / { }:
- WHY: Handles all frontend requests
- root /var/www/html: Where Angular files are
- try_files: Serves requested file or index.html (for SPA routing)

location /api/ { }:
- WHY: Forwards API requests to backend
- proxy_pass: Sends request to .NET backend on port 5000
- proxy_set_header: Preserves original request headers
- WHY NEEDED: Backend needs to know original client IP and protocol

location /health { }:
- WHY: Direct access to health check
- WHAT IT DOES: Forwards to backend health endpoint
```

#### Step 7.2: Enable Site Configuration
```bash
# WHY: Activate the new site configuration
# WHAT IT DOES: Creates symbolic link to enable site
sudo ln -s /etc/nginx/sites-available/communicaai /etc/nginx/sites-enabled/

# EXPECTED OUTPUT: No output is normal

# WHY: Remove default site configuration
# WHAT IT DOES: Disables default Nginx welcome page
sudo rm /etc/nginx/sites-enabled/default

# EXPECTED OUTPUT: No output is normal

# WHY: Test Nginx configuration for syntax errors
# WHAT IT DOES: Validates configuration file
sudo nginx -t

# EXPECTED OUTPUT: Should show:
# nginx: the configuration file /etc/nginx/nginx.conf syntax is ok
# nginx: configuration file /etc/nginx/nginx.conf test is successful

# IF ERROR:
# - Check for typos in configuration file
# - Ensure all { } brackets are balanced
# - Verify no missing semicolons

# WHY: Restart Nginx to apply changes
# WHAT IT DOES: Reloads configuration with new settings
sudo systemctl restart nginx

# EXPECTED OUTPUT: No output is normal

# WHY: Verify Nginx is running
# WHAT IT DOES: Shows service status
sudo systemctl status nginx

# EXPECTED OUTPUT: Should show "active (running)" in green
# PRESS: q to exit
```

---

### PHASE 8: Verification and Testing

#### Step 8.1: Test Complete Stack
```bash
# WHY: Verify all components are running
# WHAT IT DOES: Shows status of all services

# Check Backend
sudo systemctl status communicaai-backend

# EXPECTED: "active (running)" in green

# Check Database
sudo systemctl status postgresql

# EXPECTED: "active (running)" in green

# Check Nginx
sudo systemctl status nginx

# EXPECTED: "active (running)" in green
```

#### Step 8.2: Test Frontend Access
```
ACTION: Open web browser
URL: http://XX.XX.XX.XX (replace with your EC2 public IP)

EXPECTED RESULT:
- Should see CommunicaAI login page
- Page should load without errors
- Check browser console (F12) for any errors

IF BLANK PAGE:
- Check Nginx configuration
- Verify files in /var/www/html
- Check browser console for errors

IF "Cannot GET /":
- Nginx not configured correctly
- Check sites-enabled symlink exists
```

#### Step 8.3: Test Backend API
```
ACTION: Test API from browser
URL: http://XX.XX.XX.XX:5000/health

EXPECTED RESULT:
- Should show: {"status":"healthy"}

IF CONNECTION ERROR:
- Check backend service is running
- Check Security Group allows port 5000
- Check firewall: sudo ufw status (should be inactive)

ACTION: Test API through Nginx
URL: http://XX.XX.XX.XX/api/auth/me (should get 401 Unauthorized - this is correct)

EXPECTED RESULT:
- Should show: {"message":"Invalid token."}
- This confirms API is accessible through Nginx
```

#### Step 8.4: Test User Registration
```
ACTION: Register a new user
1. Go to: http://XX.XX.XX.XX
2. Click "Create one" or navigate to register page
3. Fill in:
   - Full Name: Test User
   - Email: test@example.com
   - Password: password123
4. Click "Create account"

EXPECTED RESULT:
- Should redirect to dashboard
- No errors in browser console

IF ERROR:
- Check browser Network tab (F12) for failed requests
- Check backend logs: sudo journalctl -u communicaai-backend -n 50
- Common issues:
  * Database connection failed (check appsettings.Production.json)
  * Gemini API error (check API key)
  * Cloudinary error (check credentials)
```

#### Step 8.5: Test Complete Interview Flow
```
ACTION: Run a test interview
1. From dashboard, click "Start Interview"
2. Select:
   - Interview Type: Technical
   - Difficulty: Easy
   - Questions: 5
   - Role: Software Engineer
3. Click "Start Interview"
4. Answer questions (you can type or record audio)
5. Click "Complete Interview"
6. Wait for results

EXPECTED RESULT:
- Questions load successfully
- Audio recording works (if testing with audio)
- Transcription appears after recording
- Results page shows scores and feedback

IF ERRORS:
- Audio transcription fails: Check Gemini API key
- Audio upload fails: Check Cloudinary credentials
- Results timeout: Check Gemini rate limits
```

---

### PHASE 9: Post-Deployment Configuration

#### Step 9.1: Setup Log Rotation
```bash
# WHY: Prevent logs from filling up disk space
# WHAT IT DOES: Automatically compresses and deletes old logs
# COMMAND: Create log rotation config
sudo nano /etc/logrotate.d/communicaai

# PASTE THIS:
```

```
/var/www/CommunicaAI/logs/*.log {
    daily
    rotate 14
    compress
    delaycompress
    missingok
    notifempty
    create 0644 ubuntu ubuntu
}
```

```bash
# SAVE AND EXIT (Ctrl+X, Y, Enter)

# EXPLANATION:
# daily: Rotate logs every day
# rotate 14: Keep 14 days of logs
# compress: Compress old logs to save space
# missingok: Don't error if log file doesn't exist
```

#### Step 9.2: Setup Automatic Security Updates
```bash
# WHY: Keep system secure automatically
# WHAT IT DOES: Installs security updates automatically
sudo apt install -y unattended-upgrades

# WHY: Enable automatic updates
sudo dpkg-reconfigure --priority=low unattended-upgrades

# WHEN PROMPTED: Select "Yes"
```

#### Step 9.3: Setup Firewall (Optional but Recommended)
```bash
# WHY: Additional security layer
# WHAT IT DOES: Controls which ports are accessible
# NOTE: This duplicates Security Group rules but adds defense in depth

# Step 1: Install UFW (Uncomplicated Firewall)
sudo apt install -y ufw

# Step 2: Set default policies
sudo ufw default deny incoming
sudo ufw default allow outgoing

# Step 3: Allow SSH (IMPORTANT - do this first!)
sudo ufw allow 22/tcp

# Step 4: Allow HTTP and HTTPS
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp

# Step 5: Allow backend API
sudo ufw allow 5000/tcp

# Step 6: Enable firewall
sudo ufw enable

# WHEN PROMPTED: Type 'y' and press Enter

# Step 7: Verify rules
sudo ufw status

# EXPECTED OUTPUT:
# Status: active
# To                         Action      From
# --                         ------      ----
# 22/tcp                     ALLOW       Anywhere
# 80/tcp                     ALLOW       Anywhere
# 443/tcp                    ALLOW       Anywhere
# 5000/tcp                   ALLOW       Anywhere
```

#### Step 9.4: Setup Monitoring Script
```bash
# WHY: Monitor application health
# WHAT IT DOES: Checks if services are running and sends alert if not
# COMMAND: Create monitoring script
nano ~/monitor.sh

# PASTE THIS:
```

```bash
#!/bin/bash

# Check if backend is running
if ! systemctl is-active --quiet communicaai-backend; then
    echo "Backend is DOWN! Restarting..."
    sudo systemctl restart communicaai-backend
fi

# Check if nginx is running
if ! systemctl is-active --quiet nginx; then
    echo "Nginx is DOWN! Restarting..."
    sudo systemctl restart nginx
fi

# Check if postgresql is running
if ! systemctl is-active --quiet postgresql; then
    echo "PostgreSQL is DOWN! Restarting..."
    sudo systemctl restart postgresql
fi

echo "All services checked at $(date)"
```

```bash
# SAVE AND EXIT

# WHY: Make script executable
chmod +x ~/monitor.sh

# WHY: Setup cron job to run every 5 minutes
crontab -e

# IF PROMPTED: Choose nano (option 1)

# ADD THIS LINE at the bottom:
*/5 * * * * /home/ubuntu/monitor.sh >> /home/ubuntu/monitor.log 2>&1

# SAVE AND EXIT

# EXPLANATION:
# */5 * * * *: Run every 5 minutes
# >> monitor.log: Append output to log file
# 2>&1: Include errors in log
```

#### Step 9.5: Allocate Elastic IP (Optional)
```bash
# WHY: Get a permanent IP address
# WHAT IT DOES: EC2 IP changes when stopped/started, Elastic IP doesn't
# BENEFIT: Can stop instance to save money without losing IP

# STEPS IN AWS CONSOLE:
1. Go to EC2 Dashboard
2. Click "Elastic IPs" in left sidebar
3. Click "Allocate Elastic IP address" button
4. Leave settings as default
5. Click "Allocate"
6. Select the newly created Elastic IP
7. Click "Actions" > "Associate Elastic IP address"
8. Select your CommunicaAI-Server instance
9. Click "Associate"

# RESULT: Instance now has permanent IP
# IMPORTANT: Update Nginx config and frontend environment with new IP

# WHY FREE TIER ELIGIBLE:
# - 1 Elastic IP is free when attached to running instance
# - Charged $0.005/hour if not attached to instance
# - Charged if you have more than 1 Elastic IP
```

---

## Post-Deployment Configuration

### Update Frontend After IP Change
```bash
# IF YOU ALLOCATED ELASTIC IP:
# WHY: Update frontend to use new permanent IP
# WHAT TO DO:

# Step 1: Update environment file
cd /var/www/CommunicaAI/Frontend
nano src/environments/environment.production.ts

# CHANGE: apiBaseUrl to new Elastic IP

# Step 2: Rebuild frontend
npm run build

# Step 3: Copy new build
sudo rm -rf /var/www/html/*
sudo cp -r dist/frontend/browser/* /var/www/html/
sudo chown -R www-data:www-data /var/www/html

# Step 4: Update Nginx config
sudo nano /etc/nginx/sites-available/communicaai

# CHANGE: server_name to new Elastic IP

# Step 5: Restart Nginx
sudo nginx -t
sudo systemctl restart nginx

# Step 6: Update CORS in backend
# Edit appsettings.Production.json if you have CORS_ORIGINS setting
```

### Setup SSL Certificate (Optional)
```bash
# WHY: Enable HTTPS for secure connections
# WHAT IT DOES: Encrypts traffic between users and server
# REQUIREMENT: Need a domain name (can't use IP address)

# IF YOU HAVE A DOMAIN:

# Step 1: Install Certbot
sudo apt install -y certbot python3-certbot-nginx

# Step 2: Get certificate
# REPLACE: yourdomain.com with your actual domain
sudo certbot --nginx -d yourdomain.com

# FOLLOW PROMPTS:
# - Enter email address
# - Agree to terms
# - Choose whether to redirect HTTP to HTTPS (recommended: Yes)

# Step 3: Test auto-renewal
sudo certbot renew --dry-run

# RESULT: HTTPS automatically configured for your domain
```

---

## Troubleshooting

### Backend Won't Start

**SYMPTOM**: `systemctl status communicaai-backend` shows "failed"

**SOLUTIONS**:
```bash
# Check detailed logs
sudo journalctl -u communicaai-backend -n 100 --no-pager

# Look for specific errors:

# 1. Database connection error
# FIX: Check appsettings.Production.json database connection string
nano /var/www/CommunicaAI/CommunicaAI/appsettings.Production.json

# 2. Port already in use
# FIX: Find and kill process using port 5000
sudo lsof -i :5000
sudo kill -9 <PID>

# 3. Permission denied
# FIX: Check file ownership
sudo chown -R ubuntu:ubuntu /var/www/CommunicaAI
sudo chmod +x /var/www/CommunicaAI/publish/CommunicaAI.dll

# 4. Missing dependencies
# FIX: Reinstall .NET SDK
sudo apt install --reinstall dotnet-sdk-8.0
```

### Frontend Shows Blank Page

**SYMPTOM**: Browser shows white screen, no content

**SOLUTIONS**:
```bash
# 1. Check browser console (F12)
# Look for JavaScript errors

# 2. Verify files exist
ls -la /var/www/html/
# Should show index.html and .js files

# 3. Check Nginx configuration
sudo nginx -t
sudo cat /etc/nginx/sites-available/communicaai

# 4. Check Nginx error log
sudo tail -f /var/log/nginx/error.log

# 5. Rebuild frontend
cd /var/www/CommunicaAI/Frontend
npm run build
sudo rm -rf /var/www/html/*
sudo cp -r dist/frontend/browser/* /var/www/html/
sudo systemctl restart nginx
```

### Database Connection Fails

**SYMPTOM**: "Connection refused" or "password authentication failed"

**SOLUTIONS**:
```bash
# 1. Check PostgreSQL is running
sudo systemctl status postgresql

# 2. Test database connection
PGPASSWORD='YourPassword' psql -h localhost -U communicaai -d CommunicaAIDB -c "SELECT 1;"

# 3. Check connection string in appsettings.Production.json
cat /var/www/CommunicaAI/CommunicaAI/appsettings.Production.json | grep ConnectionStrings

# 4. Reset database password if needed
sudo -u postgres psql
ALTER USER communicaai WITH PASSWORD 'NewPassword';
\q

# Then update appsettings.Production.json with new password
```

### API Requests Fail with CORS Error

**SYMPTOM**: Browser console shows "CORS policy" error

**SOLUTIONS**:
```bash
# 1. Check backend CORS configuration in Program.cs
# Should allow your frontend IP/domain

# 2. Verify frontend is making requests to correct API URL
cat /var/www/CommunicaAI/Frontend/src/environments/environment.production.ts

# 3. Check Nginx proxy headers
sudo cat /etc/nginx/sites-available/communicaai
# Should have proxy_set_header lines

# 4. Restart services
sudo systemctl restart communicaai-backend
sudo systemctl restart nginx
```

### Out of Memory Errors

**SYMPTOM**: npm install or npm run build fails with "ENOMEM"

**SOLUTIONS**:
```bash
# Option 1: Create swap file (adds virtual memory)
sudo fallocate -l 2G /swapfile
sudo chmod 600 /swapfile
sudo mkswap /swapfile
sudo swapon /swapfile

# Make swap permanent
echo '/swapfile none swap sw 0 0' | sudo tee -a /etc/fstab

# Option 2: Build frontend locally and copy
# On your local machine:
npm run build
scp -i communicaai-key.pem -r dist/frontend/browser/* ubuntu@XX.XX.XX.XX:/tmp/

# On EC2:
sudo rm -rf /var/www/html/*
sudo mv /tmp/browser/* /var/www/html/
sudo chown -R www-data:www-data /var/www/html
```

---

## Cost Management

### Staying Within Free Tier

**Free Tier Limits (First 12 Months)**:
- 750 hours/month of t2.micro EC2 instance
- 30 GB of EBS storage
- 15 GB of bandwidth out per month
- 1 Elastic IP (when attached to running instance)

**How to Stay Free**:
1. **Don't exceed 750 hours/month**: Run only 1 t2.micro instance
2. **Monitor storage**: Keep under 30 GB total
3. **Monitor bandwidth**: Limit large file transfers
4. **Stop instance when not needed**: 
   ```bash
   # Stop instance (but you'll lose public IP)
   # Do this from AWS Console:
   Actions > Instance State > Stop
   ```

### Monitoring Costs

**Setup Billing Alerts**:
1. Go to AWS Console > Billing Dashboard
2. Click "Billing Preferences" in left sidebar
3. Check "Receive Free Tier Usage Alerts"
4. Enter your email address
5. Click "Save preferences"
6. Go to "Budgets" in left sidebar
7. Create Budget:
   - Budget type: Cost budget
   - Set budget amount: $1 (or any amount)
   - Alert threshold: 80%
   - Email: your email

**Check Usage**:
- Go to AWS Console > Billing Dashboard
- Click "Free Tier" in left sidebar
- Review usage of each service

**What Happens After 12 Months**:
- Free tier expires
- You'll be charged normal rates (~$8-10/month for t2.micro)
- Decide to continue paying or migrate to other free hosting

---

## Useful Commands Reference

### Service Management
```bash
# Backend service
sudo systemctl start communicaai-backend      # Start
sudo systemctl stop communicaai-backend       # Stop
sudo systemctl restart communicaai-backend    # Restart
sudo systemctl status communicaai-backend     # Check status
sudo journalctl -u communicaai-backend -f     # View live logs

# Nginx
sudo systemctl start nginx
sudo systemctl stop nginx
sudo systemctl restart nginx
sudo systemctl status nginx
sudo tail -f /var/log/nginx/error.log        # View error logs
sudo tail -f /var/log/nginx/access.log       # View access logs

# PostgreSQL
sudo systemctl start postgresql
sudo systemctl stop postgresql
sudo systemctl restart postgresql
sudo systemctl status postgresql
```

### Application Management
```bash
# Update application code
cd /var/www/CommunicaAI
git pull origin master

# Rebuild backend
cd CommunicaAI
dotnet publish -c Release -o /var/www/CommunicaAI/publish
sudo systemctl restart communicaai-backend

# Rebuild frontend
cd ../Frontend
npm run build
sudo rm -rf /var/www/html/*
sudo cp -r dist/frontend/browser/* /var/www/html/
sudo chown -R www-data:www-data /var/www/html

# Apply database migrations
cd /var/www/CommunicaAI/CommunicaAI
dotnet ef database update
```

### System Monitoring
```bash
# Check disk space
df -h

# Check memory usage
free -h

# Check CPU usage
top
# Press q to exit

# Check running processes
ps aux | grep dotnet
ps aux | grep nginx

# Check listening ports
sudo netstat -tulpn | grep LISTEN

# Check system load
uptime
```

### Database Management
```bash
# Connect to database
sudo -u postgres psql CommunicaAIDB

# Backup database
sudo -u postgres pg_dump CommunicaAIDB > backup.sql

# Restore database
sudo -u postgres psql CommunicaAIDB < backup.sql

# Check database size
sudo -u postgres psql -c "SELECT pg_database.datname, pg_size_pretty(pg_database_size(pg_database.datname)) FROM pg_database;"
```

---

## Security Best Practices

### 1. Keep System Updated
```bash
# Run weekly
sudo apt update && sudo apt upgrade -y
```

### 2. Change Default PostgreSQL Passwords
```bash
# Change postgres user password
sudo -u postgres psql
ALTER USER postgres WITH PASSWORD 'NewStrongPassword!';
\q
```

### 3. Disable Root SSH Login
```bash
sudo nano /etc/ssh/sshd_config
# Change: PermitRootLogin no
sudo systemctl restart sshd
```

### 4. Setup Fail2Ban (Prevents Brute Force)
```bash
sudo apt install -y fail2ban
sudo systemctl enable fail2ban
sudo systemctl start fail2ban
```

### 5. Regular Backups
```bash
# Create backup script
nano ~/backup.sh

# Add:
#!/bin/bash
DATE=$(date +%Y%m%d_%H%M%S)
sudo -u postgres pg_dump CommunicaAIDB > ~/backups/db_$DATE.sql
tar -czf ~/backups/app_$DATE.tar.gz /var/www/CommunicaAI

# Make executable
chmod +x ~/backup.sh

# Run daily via cron
crontab -e
# Add: 0 2 * * * /home/ubuntu/backup.sh
```

---

## Conclusion

You now have CommunicaAI fully deployed on AWS EC2 Free Tier!

**What You've Accomplished**:
- ✅ Created and configured EC2 instance
- ✅ Installed all required software
- ✅ Deployed backend .NET API
- ✅ Deployed frontend Angular application
- ✅ Configured PostgreSQL database
- ✅ Setup Nginx reverse proxy
- ✅ Configured system services
- ✅ Implemented monitoring and logging

**Next Steps**:
1. Test all features thoroughly
2. Setup custom domain (optional)
3. Enable HTTPS with Let's Encrypt (optional)
4. Configure regular backups
5. Monitor AWS billing dashboard

**Support**:
- AWS Documentation: https://docs.aws.amazon.com/
- AWS Free Tier: https://aws.amazon.com/free/
- Ubuntu Documentation: https://help.ubuntu.com/

---

**Deployment Date**: _____________
**EC2 Instance ID**: _____________
**Public IP**: _____________
**Elastic IP** (if allocated): _____________

---

**END OF DEPLOYMENT GUIDE**
