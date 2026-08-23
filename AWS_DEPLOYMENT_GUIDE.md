# CommunicaAI AWS Deployment Guide

This guide deploys CommunicaAI to AWS with a production-friendly split:

- ASP.NET Core API in ECS Fargate or App Runner
- PostgreSQL in RDS
- Angular frontend in S3 + CloudFront
- Secrets in AWS Systems Manager Parameter Store or Secrets Manager

## Architecture

Recommended AWS layout:

- `Frontend` static site: S3 + CloudFront
- `CommunicaAI` API: ECS Fargate behind an Application Load Balancer, or App Runner for a simpler container deployment
- Database: Amazon RDS PostgreSQL
- Secrets: AWS Secrets Manager or Systems Manager Parameter Store
- Logs: CloudWatch Logs
- DNS and TLS: Route 53 + ACM

## What this repo already provides

- Dockerfile for the backend container
- `/health` and `/` endpoints for load balancer checks
- PostgreSQL support through `DATABASE_URL` or a standard connection string
- Environment-driven CORS support through `FRONTEND_ORIGINS` or `CORS_ORIGINS`
- Optional Swagger exposure controlled by `ENABLE_SWAGGER`
- Angular production build output under `Frontend/dist/Frontend`

## 1. Prerequisites

You need these AWS resources or equivalents:

- An AWS account with permissions for ECS, ECR, RDS, S3, CloudFront, Route 53, ACM, CloudWatch, IAM
- A registered domain if you want custom URLs
- A PostgreSQL database in RDS
- A private container registry in ECR

## 2. Backend configuration

Set these values in the AWS service that runs the API:

- `ASPNETCORE_ENVIRONMENT=Production`
- `PORT=10000`
- `DATABASE_URL=postgresql://username:password@host:5432/database?sslmode=require`
- `Jwt__Issuer=CommunicaAI`
- `Jwt__Audience=CommunicaAIUsers`
- `Jwt__Key=<long-random-secret>`
- `CloudinarySettings__CloudName=<value>`
- `CloudinarySettings__ApiKey=<value>`
- `CloudinarySettings__ApiSecret=<value>`
- `Gemini__ApiKey=<value>`
- `Gemini__Model=gemini-2.5-flash`
- `PythonVerificationService__BaseUrl=<https://your-python-service>`
- `FRONTEND_ORIGINS=https://your-cloudfront-domain,https://your-custom-domain`
- `ENABLE_SWAGGER=false`

Notes:

- Use AWS Secrets Manager or Parameter Store for the secret values.
- `FRONTEND_ORIGINS` can contain multiple comma-separated origins.
- If you need Swagger in a private environment, set `ENABLE_SWAGGER=true` temporarily.

## 3. Database setup

1. Create an RDS PostgreSQL instance.
2. Put the instance in the same VPC as the API service if possible.
3. Allow inbound traffic only from the API security group.
4. Save the connection string as `DATABASE_URL` or `ConnectionStrings__DefaultConnection`.
5. Run the EF Core migrations against the RDS database before first launch.

Suggested local migration command:

```bash
cd CommunicaAI
dotnet ef database update
```

If you prefer running migrations from the deployment host, use the same connection string that the service will use in production.

## 4. Backend deployment

### Option A: ECS Fargate

1. Build the API container.
2. Push the image to ECR.
3. Create an ECS task definition using the image.
4. Expose container port `10000`.
5. Put the service behind an Application Load Balancer.
6. Configure the ALB health check path as `/health`.
7. Attach CloudWatch Logs to the task definition.

### Option B: App Runner

1. Push the API image to ECR.
2. Create an App Runner service from the image.
3. Set the container port to `10000`.
4. Add the environment variables listed above.
5. Configure the health check path as `/health`.

### Build and push example

From the backend folder:

```bash
docker build -t communicaai-api .
docker tag communicaai-api:latest <account-id>.dkr.ecr.<region>.amazonaws.com/communicaai-api:latest
docker push <account-id>.dkr.ecr.<region>.amazonaws.com/communicaai-api:latest
```

## 5. Frontend configuration

The Angular app reads its backend URL from `Frontend/src/environments/environment.production.ts` during the production build.

Before building for AWS, update the production API base URL to your deployed API endpoint. For example:

```ts
export const environment = {
  production: true,
  apiBaseUrl: 'https://api.your-domain.com'
};
```

If you deploy the frontend and backend behind the same domain with path-based routing, you can instead point the frontend at the API path used by your proxy setup.

## 6. Frontend deployment

### Build

```bash
cd Frontend
npm ci
npm run build
```

### Publish to S3

1. Create an S3 bucket for the Angular build output.
2. Upload the contents of `dist/Frontend` to the bucket.
3. Enable CloudFront with the S3 bucket as the origin.
4. Configure the distribution for SPA routing so unknown paths fall back to `index.html`.
5. Attach your ACM certificate if you use a custom domain.

### Useful CloudFront settings

- Default root object: `index.html`
- Cache policy: standard static asset caching
- Error response: map `403` and `404` to `/index.html` for client-side routing

## 7. DNS and TLS

1. Create Route 53 records for the frontend domain and API domain.
2. Issue ACM certificates for the domains in the same AWS region required by the service.
3. Attach the certificate to CloudFront for the frontend and to the ALB if you use one for the API.

## 8. Validation after deployment

Check these URLs first:

- `https://your-api-domain/health`
- `https://your-api-domain/`
- `https://your-frontend-domain/`

Then verify the app flow:

1. Register or log in.
2. Confirm API requests succeed without CORS errors.
3. Confirm database-backed pages load data.
4. Confirm file uploads and interview flows still work.

## 9. Troubleshooting

### CORS errors

- Make sure `FRONTEND_ORIGINS` includes the exact frontend origin.
- Include the scheme, host, and port if applicable.
- Restart the API service after changing the environment variable.

### Database connection errors

- Confirm the RDS security group allows the API to connect.
- Confirm `DATABASE_URL` uses the correct host, database name, and credentials.
- Use `sslmode=require` for RDS PostgreSQL.

### API returns 502 or health check failures

- Confirm the container listens on port `10000`.
- Confirm the load balancer or App Runner health check points to `/health`.

### Frontend opens but API calls fail

- Confirm `environment.production.ts` points to the deployed API URL.
- Confirm the API domain is reachable from the browser.
- Confirm CORS allows the frontend origin.

## 10. Recommended rollout order

1. Deploy RDS PostgreSQL.
2. Deploy the backend API container.
3. Confirm `/health` works.
4. Build and publish the Angular frontend.
5. Update DNS and TLS.
6. Run a full user flow test.

## 11. Local commands that mirror production

Backend:

```bash
cd CommunicaAI
dotnet build
dotnet run
```

Frontend:

```bash
cd Frontend
npm ci
npm run build
```

If you want, keep this guide next to the repo root so it stays visible during AWS rollout.
