# Deploying Clothing E-Shop to Render

This guide provides step-by-step instructions for deploying your Blazor application to Render with PostgreSQL database.

## ?? Prerequisites

- GitHub account with your code pushed to a repository
- Render account (free tier available at https://render.com)
- Git repository with all changes committed

## ?? Deployment Steps

### Step 1: Prepare Your Repository

1. **Ensure all files are committed to Git:**
   ```bash
   git add .
   git commit -m "Prepare for Render deployment"
   git push origin main
   ```

2. **Verify these files exist in your repository:**
   - ? `render.yaml` - Render configuration file
   - ? `Dockerfile` - Docker configuration
   - ? `.dockerignore` - Docker ignore file

### Step 2: Create a Render Account

1. Go to https://render.com
2. Click "Get Started" or "Sign Up"
3. Sign up with GitHub (recommended for easy deployment)
4. Authorize Render to access your repositories

### Step 3: Deploy Using render.yaml (Recommended)

#### Option A: Deploy via Blueprint (Automatic)

1. **Go to Render Dashboard:**
   - Navigate to https://dashboard.render.com

2. **Create New Blueprint Instance:**
   - Click "New +" in the top navigation
   - Select "Blueprint"

3. **Connect Repository:**
   - Select your GitHub repository
   - Choose the repository containing your ClothingEShop project
   - Click "Connect"

4. **Configure Blueprint:**
   - Render will automatically detect your `render.yaml` file
   - Review the services that will be created:
     - PostgreSQL Database: `clothingeshop-db`
     - Web Service: `clothingeshop-app`

5. **Deploy:**
   - Click "Apply"
   - Render will start creating your services
   - This process takes 5-10 minutes

#### Option B: Manual Service Creation

If you prefer manual setup or the blueprint fails:

##### 1. Create PostgreSQL Database

1. **From Dashboard, click "New +" ? "PostgreSQL"**
2. **Configure Database:**
   - Name: `clothingeshop-db`
   - Database: `clothingeshop`
   - User: `clothingeshop_user`
   - Region: Choose closest to your users
   - Plan: Free
3. **Click "Create Database"**
4. **Wait for database creation** (2-3 minutes)
5. **Copy the Internal Database URL** (you'll need this)

##### 2. Create Web Service

1. **From Dashboard, click "New +" ? "Web Service"**
2. **Connect Repository:**
   - Select "Build and deploy from a Git repository"
   - Connect your GitHub account if not already connected
   - Select your repository
   - Click "Connect"

3. **Configure Web Service:**
   - **Name:** `clothingeshop-app`
   - **Region:** Same as your database
   - **Branch:** `main` (or your default branch)
   - **Root Directory:** Leave blank (or specify if in subdirectory)
   - **Environment:** `Docker`
   - **Dockerfile Path:** `./Dockerfile`
   - **Docker Build Context Directory:** `.`
   - **Plan:** Free

4. **Add Environment Variables:**
   Click "Advanced" and add these environment variables:
   
   | Key | Value |
   |-----|-------|
   | `ASPNETCORE_ENVIRONMENT` | `Production` |
   | `ASPNETCORE_URLS` | `http://+:10000` |
   | `ConnectionStrings__DefaultConnection` | (Your database internal URL) |

   **For the database connection string:**
   - Go to your PostgreSQL database page
   - Copy the "Internal Database URL"
   - Paste it as the value for `ConnectionStrings__DefaultConnection`
   - Format: `postgresql://user:password@host:5432/database`

5. **Health Check Path (Optional but Recommended):**
   - Add `/health` as the health check path

6. **Click "Create Web Service"**

### Step 4: Monitor Deployment

1. **View Build Logs:**
   - Once created, you'll see the deployment logs
   - Watch for any errors during:
     - Docker image build
     - Container startup
     - Database connection

2. **Common Build Stages:**
   ```
   ==> Building... (5-8 minutes)
   ==> Deploying... (1-2 minutes)  
   ==> Live ?
   ```

3. **Check Deployment Status:**
   - Green checkmark = Successful deployment
   - Red X = Failed deployment (check logs)

### Step 5: Verify Deployment

1. **Access Your Application:**
   - Render provides a URL like: `https://clothingeshop-app.onrender.com`
   - Click the URL to open your application

2. **Test Key Functionality:**
   - ? Homepage loads
   - ? Products page displays
   - ? Can create a new product
   - ? Can view product details
   - ? Can edit products
   - ? Can delete products
   - ? Search functionality works
   - ? API endpoints respond: `/api/products`

3. **Check Health Endpoint:**
   - Visit: `https://your-app.onrender.com/health`
   - Should return: `Healthy`

### Step 6: Update README with URLs

Update your `README.md` with the deployed URLs:

```markdown
## ?? Live Demo

- **Application:** https://clothingeshop-app.onrender.com
- **API:** https://clothingeshop-app.onrender.com/api/products
- **Health Check:** https://clothingeshop-app.onrender.com/health
```

## ?? Environment Variables Reference

| Variable | Description | Example Value |
|----------|-------------|---------------|
| `ASPNETCORE_ENVIRONMENT` | ASP.NET environment | `Production` |
| `ASPNETCORE_URLS` | URLs to listen on | `http://+:10000` |
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | `postgresql://user:pass@host:5432/db` |

## ?? Continuous Deployment

Render automatically deploys when you push to your main branch:

```bash
git add .
git commit -m "Update feature"
git push origin main
```

Render will:
1. Detect the push
2. Trigger a new build
3. Deploy automatically (if build succeeds)

### Disable Auto-Deploy (Optional)

In your web service settings:
- Go to "Settings" tab
- Under "Build & Deploy"
- Toggle "Auto-Deploy" off

## ?? Troubleshooting

### Issue 1: Database Connection Failed

**Symptoms:**
- App starts but crashes when accessing products
- Logs show: "Connection refused" or "Unable to connect to database"

**Solutions:**
1. **Verify Connection String:**
   - Go to Database ? "Connect" tab
   - Copy "Internal Database URL"
   - Update environment variable in Web Service

2. **Check Database Status:**
   - Ensure database shows "Available" status
   - Wait if it's still provisioning

3. **Connection String Format:**
   - Render format: `postgresql://user:password@host:5432/database`
   - If using .NET format, ensure proper conversion

### Issue 2: Build Fails

**Symptoms:**
- Build never completes
- Errors in build logs

**Solutions:**
1. **Check Dockerfile:**
   - Ensure Dockerfile is in root directory
   - Verify COPY paths are correct

2. **Check .dockerignore:**
   - Ensure it's not excluding necessary files

3. **Review Build Logs:**
   - Look for specific error messages
   - Common issues: missing dependencies, wrong paths

### Issue 3: Application Crashes on Startup

**Symptoms:**
- Build succeeds but app immediately crashes
- "Application failed to start" error

**Solutions:**
1. **Check Logs:**
   - Click "Logs" tab
   - Look for startup errors

2. **Port Configuration:**
   - Verify `ASPNETCORE_URLS=http://+:10000`
   - Render requires port 10000

3. **Database Migrations:**
   - Ensure database is initialized
   - Check if `EnsureCreated()` runs successfully

### Issue 4: 502 Bad Gateway

**Symptoms:**
- URL loads but shows 502 error
- Service shows as "Live" but not accessible

**Solutions:**
1. **Health Check:**
   - Remove health check path temporarily
   - Or ensure `/health` endpoint works

2. **Restart Service:**
   - Go to service settings
   - Click "Manual Deploy" ? "Clear build cache & deploy"

### Issue 5: Free Tier Limitations

**Symptoms:**
- Service spins down after inactivity
- First request is slow (cold start)

**Understanding Free Tier:**
- Free services spin down after 15 minutes of inactivity
- Cold starts take 30-60 seconds
- Database remains active

**Workarounds:**
1. **Upgrade to Paid Plan** ($7/month) - keeps service running
2. **Use a Ping Service:**
   - https://uptimerobot.com (free)
   - Ping your app every 10 minutes
3. **Accept cold starts** for hobby projects

### Issue 6: Environment Variables Not Loading

**Solutions:**
1. **Check Variable Names:**
   - Must use double underscore: `ConnectionStrings__DefaultConnection`
   - Case-sensitive

2. **Re-deploy After Changes:**
   - Environment variable changes require manual deploy
   - Click "Manual Deploy" ? "Deploy latest commit"

## ?? Monitoring Your Application

### View Logs

1. **Real-time Logs:**
   - Go to your web service
   - Click "Logs" tab
   - Watch live application logs

2. **Filter Logs:**
   - Use search box to filter
   - Look for "Error", "Exception", "Failed"

### Metrics (Paid Plans)

- CPU usage
- Memory usage  
- Response times
- Request counts

## ?? Cost Considerations

### Free Tier Includes:
- ? 750 hours/month for web services
- ? PostgreSQL database (90 days, then deleted if inactive)
- ? 100 GB bandwidth
- ? Automatic SSL certificates
- ? Custom domains

### Free Tier Limitations:
- ?? Services spin down after 15 minutes of inactivity
- ?? 512 MB RAM for web service
- ?? 1 GB disk for database
- ?? Slower build times

### Upgrade Options:
- **Starter Plan:** $7/month per service (no spin down)
- **PostgreSQL:** $7/month (persistent, 1 GB storage)

## ?? Security Best Practices

1. **Never Commit Sensitive Data:**
   - Keep `.env` in `.gitignore`
   - Use Render's environment variables

2. **Use Render's Secrets:**
   - Store sensitive values as environment variables
   - They're encrypted at rest

3. **Database Security:**
   - Use the Internal Database URL (more secure)
   - External URL only if accessing from outside Render

4. **HTTPS:**
   - Render provides free SSL certificates
   - All traffic automatically uses HTTPS

## ?? Pre-Deployment Checklist

Before deploying to Render, ensure:

- [ ] Code is pushed to GitHub
- [ ] `render.yaml` exists and is configured
- [ ] `Dockerfile` exposes port 10000
- [ ] `.gitignore` excludes `.env` files
- [ ] Database connection uses environment variables
- [ ] Health check endpoint (`/health`) works
- [ ] Application runs successfully in Docker locally
- [ ] All migrations/seed data scripts are ready
- [ ] README.md has deployment documentation

## ?? Post-Deployment Steps

1. **Test Thoroughly:**
   - Test all CRUD operations
   - Check API endpoints
   - Verify search functionality
   - Test on mobile devices

2. **Monitor Performance:**
   - Check response times
   - Monitor error logs
   - Watch for memory issues

3. **Set Up Monitoring (Optional):**
   - Use UptimeRobot for uptime monitoring
   - Set up alerts for downtime

4. **Update Documentation:**
   - Add live URL to README
   - Document any production-specific notes
   - Update API documentation with live endpoints

## ?? Additional Resources

- **Render Documentation:** https://render.com/docs
- **Docker on Render:** https://render.com/docs/docker
- **PostgreSQL on Render:** https://render.com/docs/databases
- **Environment Variables:** https://render.com/docs/environment-variables
- **Custom Domains:** https://render.com/docs/custom-domains

## ?? Getting Help

If you encounter issues:

1. **Check Render Status:** https://status.render.com
2. **Render Community:** https://community.render.com
3. **Render Support:** support@render.com (paid plans)
4. **Stack Overflow:** Tag questions with `render` and `blazor`

---

**Note:** First deployment takes 5-10 minutes. Subsequent deployments are faster (2-5 minutes).

Good luck with your deployment! ??
