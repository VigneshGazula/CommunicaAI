# CommunicaAI - Render Deployment Guide

Complete guide to deploy the CommunicaAI application (Angular frontend + ASP.NET Core backend) to Render.

---

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Backend Deployment (ASP.NET Core)](#backend-deployment)
3. [Frontend Deployment (Angular)](#frontend-deployment)
4. [Environment Variables](#environment-variables)
5. [Database Setup](#database-setup)
6. [Post-Deployment](#post-deployment)
7. [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Required Accounts
- ✅ [Render Account](https://render.com/) (free tier available)
- ✅ [GitHub Account](https://github.com/) (for code repository)
- ✅ OpenAI API Key (for interview functionality)

### Required Tools
- Git installed locally
- GitHub repository with your code

### Project Structure
```
CommunicaAI/
├── Frontend/          # Angular application
│   ├── src/
│   ├── package.json
│   ├── angular.json
│   └── ...
└── CommunicaAI/      # ASP.NET Core backend
    ├── Controllers/
    ├── Models/
    ├── appsettings.json
    └── ...
```

---

## Backend Deployment (ASP.NET Core)

### Step 1: Prepare Backend for Deployment

#### 1.1 Create Dockerfile in Backend Root

Create `CommunicaAI/Dockerfile`:

```dockerfile
# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["CommunicaAI.csproj", "./"]
RUN dotnet restore "CommunicaAI.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "CommunicaAI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "CommunicaAI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommunicaAI.dll"]
```

#### 1.2 Update appsettings.json

Make sure your `appsettings.json` uses environment variables:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "${DATABASE_URL}"
  },
  "OpenAI": {
    "ApiKey": "${OPENAI_API_KEY}",
    "Model": "gpt-4"
  },
  "Cors": {
    "AllowedOrigins": "${CORS_ORIGINS}"
  }
}
```

#### 1.3 Update Program.cs for Production

Ensure your `Program.cs` has proper CORS and production settings:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// CORS
var corsOrigins = Environment.GetEnvironmentVariable("CORS_ORIGINS")
    ?? "http://localhost:4200";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins(corsOrigins.Split(','))
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Step 2: Push Backend to GitHub

```bash
cd CommunicaAI
git init
git add .
git commit -m "Initial backend commit"
git branch -M main
git remote add origin https://github.com/YOUR_USERNAME/communicaai-backend.git
git push -u origin main
```

### Step 3: Deploy Backend on Render

1. **Log in to Render Dashboard**
   - Go to https://dashboard.render.com/

2. **Create New Web Service**
   - Click "New +" → "Web Service"

3. **Connect Repository**
   - Connect your GitHub account
   - Select `communicaai-backend` repository
   - Click "Connect"

4. **Configure Web Service**
   - **Name**: `communicaai-api`
   - **Region**: Choose closest to your users
   - **Branch**: `main`
   - **Root Directory**: `CommunicaAI` (if monorepo) or leave blank
   - **Environment**: `Docker`
   - **Instance Type**: Free (or paid for better performance)

5. **Add Environment Variables**
   Click "Advanced" → "Add Environment Variable":
   
   ```
   DATABASE_URL=postgresql://user:password@host:5432/dbname
   OPENAI_API_KEY=sk-your-openai-key-here
   CORS_ORIGINS=https://your-frontend-url.onrender.com
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ```

6. **Deploy**
   - Click "Create Web Service"
   - Wait for deployment (5-10 minutes)
   - Note your backend URL: `https://communicaai-api.onrender.com`

---

## Frontend Deployment (Angular)

### Step 1: Prepare Frontend for Deployment

#### 1.1 Update Environment Files

Update `Frontend/src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://communicaai-api.onrender.com/api'
};
```

Update `Frontend/src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

#### 1.2 Create Build Script

Create `Frontend/render-build.sh`:

```bash
#!/bin/bash
# Install dependencies
npm ci

# Build Angular app for production
npm run build

# Install serve globally for serving static files
npm install -g serve

echo "Build completed successfully!"
```

Make it executable:
```bash
chmod +x render-build.sh
```

#### 1.3 Create Start Script

Create `Frontend/render-start.sh`:

```bash
#!/bin/bash
# Serve the built Angular app
serve -s dist/Frontend -l 3000
```

Make it executable:
```bash
chmod +x render-start.sh
```

#### 1.4 Update package.json

Add these scripts to `Frontend/package.json`:

```json
{
  "scripts": {
    "ng": "ng",
    "start": "ng serve",
    "build": "ng build",
    "watch": "ng build --watch --configuration development",
    "test": "ng test",
    "render-build": "./render-build.sh",
    "render-start": "./render-start.sh"
  }
}
```

### Step 2: Push Frontend to GitHub

```bash
cd Frontend
git init
git add .
git commit -m "Initial frontend commit"
git branch -M main
git remote add origin https://github.com/YOUR_USERNAME/communicaai-frontend.git
git push -u origin main
```

### Step 3: Deploy Frontend on Render

1. **Create New Static Site**
   - Click "New +" → "Static Site"

2. **Connect Repository**
   - Select `communicaai-frontend` repository
   - Click "Connect"

3. **Configure Static Site**
   - **Name**: `communicaai`
   - **Branch**: `main`
   - **Root Directory**: Leave blank
   - **Build Command**: `npm install && npm run build`
   - **Publish Directory**: `dist/Frontend/browser`

4. **Add Environment Variables**
   ```
   NODE_VERSION=20.11.0
   ```

5. **Deploy**
   - Click "Create Static Site"
   - Wait for deployment (5-10 minutes)
   - Note your frontend URL: `https://communicaai.onrender.com`

### Step 4: Update Backend CORS

Go back to your backend service on Render and update the `CORS_ORIGINS` environment variable:

```
CORS_ORIGINS=https://communicaai.onrender.com
```

Click "Save Changes" - this will redeploy the backend.

---

## Database Setup

### Option 1: Render PostgreSQL (Recommended)

1. **Create PostgreSQL Database**
   - In Render Dashboard, click "New +" → "PostgreSQL"
   - **Name**: `communicaai-db`
   - **Database**: `communicaai`
   - **User**: `communicaai_user`
   - **Region**: Same as your backend
   - **Instance Type**: Free tier
   - Click "Create Database"

2. **Get Connection String**
   - After creation, copy the "External Database URL"
   - Format: `postgresql://user:password@host:5432/database`

3. **Update Backend Environment Variable**
   - Go to your backend web service
   - Update `DATABASE_URL` with the connection string
   - Click "Save Changes"

4. **Run Migrations**
   - After backend deploys, it should auto-run migrations
   - Or manually run via Render shell:
   ```bash
   dotnet ef database update
   ```

### Option 2: External Database (Supabase/Neon)

If using external PostgreSQL:

1. Create database on [Supabase](https://supabase.com/) or [Neon](https://neon.tech/)
2. Get connection string
3. Update `DATABASE_URL` in Render backend environment variables

---

## Environment Variables

### Backend Environment Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `DATABASE_URL` | PostgreSQL connection string | `postgresql://user:pass@host:5432/db` |
| `OPENAI_API_KEY` | OpenAI API key | `sk-proj-...` |
| `CORS_ORIGINS` | Allowed frontend URLs | `https://your-app.onrender.com` |
| `ASPNETCORE_ENVIRONMENT` | Environment mode | `Production` |
| `ASPNETCORE_URLS` | Port binding | `http://+:8080` |

### Frontend Environment Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `NODE_VERSION` | Node.js version | `20.11.0` |

---

## Post-Deployment

### 1. Verify Backend

Test your backend API:
```bash
curl https://communicaai-api.onrender.com/api/health
```

### 2. Verify Frontend

Visit your frontend URL:
```
https://communicaai.onrender.com
```

### 3. Test Full Flow

1. Register a new user
2. Create an interview setup
3. Start an interview
4. Test recording functionality
5. View results

### 4. Monitor Logs

- **Backend Logs**: Render Dashboard → Your Web Service → Logs
- **Frontend Logs**: Render Dashboard → Your Static Site → Logs

### 5. Set Up Custom Domain (Optional)

1. **In Render Dashboard**
   - Go to your service → Settings → Custom Domains
   - Click "Add Custom Domain"
   - Enter your domain (e.g., `app.communicaai.com`)

2. **Update DNS**
   - Add CNAME record pointing to Render:
   ```
   CNAME app communicaai.onrender.com
   ```

3. **SSL Certificate**
   - Render automatically provisions SSL certificates
   - Wait 5-10 minutes for DNS propagation

---

## Troubleshooting

### Backend Issues

#### Issue: Database Connection Failed
**Solution:**
```bash
# Check connection string format
# Ensure PostgreSQL is running
# Verify network access from Render
```

#### Issue: CORS Errors
**Solution:**
- Update `CORS_ORIGINS` environment variable
- Include protocol (https://)
- Redeploy backend after changes

#### Issue: OpenAI API Errors
**Solution:**
- Verify API key is correct
- Check billing status on OpenAI
- Ensure key has proper permissions

### Frontend Issues

#### Issue: Build Fails
**Solution:**
```bash
# Check Node version compatibility
# Verify all dependencies in package.json
# Check build logs for specific errors
```

#### Issue: API Calls Failing
**Solution:**
- Verify `apiUrl` in environment.prod.ts
- Check browser console for CORS errors
- Ensure backend is deployed and running

#### Issue: Routes Not Working (404)
**Solution:**
Add `_redirects` file in `Frontend/src/_redirects`:
```
/*    /index.html   200
```

Update `angular.json`:
```json
{
  "architect": {
    "build": {
      "options": {
        "assets": [
          "src/favicon.ico",
          "src/assets",
          "src/_redirects"
        ]
      }
    }
  }
}
```

---

## Quick Deploy Checklist

### Before Deployment
- [ ] Code pushed to GitHub
- [ ] Environment files configured
- [ ] Database schema ready
- [ ] API keys obtained
- [ ] Build scripts tested locally

### Backend Deployment
- [ ] Dockerfile created
- [ ] Service configured on Render
- [ ] Environment variables set
- [ ] Database connected
- [ ] Migrations run
- [ ] API endpoints tested

### Frontend Deployment
- [ ] Production environment configured
- [ ] Build command verified
- [ ] Static site created on Render
- [ ] Environment variables set
- [ ] Deployment successful
- [ ] Routes working

### Post-Deployment
- [ ] Full application flow tested
- [ ] CORS configured correctly
- [ ] SSL certificate active
- [ ] Monitoring set up
- [ ] Backups verified

---

## Cost Estimation

### Free Tier (Starter)
- Backend: Free (with limitations)
- Frontend: Free
- Database: Free (1GB)
- **Total: $0/month**

### Paid Tier (Production)
- Backend: $7/month (Starter)
- Database: $7/month (Starter)
- Frontend: Free (static site)
- **Total: $14/month**

---

## Conclusion

Your CommunicaAI application is now deployed on Render! 🎉

**Live URLs:**
- Frontend: `https://communicaai.onrender.com`
- Backend API: `https://communicaai-api.onrender.com`

For questions or issues, refer to the troubleshooting section or Render support.

---

**Last Updated:** July 9, 2026  
**Version:** 1.0.0
