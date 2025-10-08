# Render Deployment - Quick Start Guide

## ? Quick Deploy (5 Minutes)

### 1. Push to GitHub
```bash
git add .
git commit -m "Ready for Render deployment"
git push origin main
```

### 2. Create Render Account
- Go to https://render.com
- Sign up with GitHub
- Authorize Render

### 3. Deploy with Blueprint
1. Click **"New +" ? "Blueprint"**
2. Connect your repository
3. Render detects `render.yaml`
4. Click **"Apply"**
5. Wait 5-10 minutes ??

### 4. Get Your URLs
- **App:** `https://clothingeshop-app.onrender.com`
- **API:** `https://clothingeshop-app.onrender.com/api/products`
- **Health:** `https://clothingeshop-app.onrender.com/health`

## ? What Gets Deployed

- ??? **PostgreSQL Database** (Free tier, 1GB)
- ?? **Blazor Web App** (Dockerized, 512MB RAM)
- ?? **Automatic SSL** (HTTPS enabled)
- ?? **Auto-deploy** on GitHub push

## ?? Important Notes

### Free Tier Behavior
- ?? App sleeps after 15 min of inactivity
- ?? Cold start: 30-60 seconds for first request
- ?? Database: 90 days retention (free tier)
- ?? 750 hours/month service time

### Port Configuration
- ? App configured for port **10000** (Render's default)
- ? Dockerfile updated to expose 10000
- ? Environment variable: `ASPNETCORE_URLS=http://+:10000`

## ?? Environment Variables (Auto-configured)

Render automatically sets these from `render.yaml`:
- `ASPNETCORE_ENVIRONMENT=Production`
- `ASPNETCORE_URLS=http://+:10000`
- `ConnectionStrings__DefaultConnection` (from PostgreSQL service)

## ?? Common Issues & Fixes

### Issue: App won't start
**Fix:** Check logs for database connection errors
```
Dashboard ? Your Service ? Logs
```

### Issue: 502 Bad Gateway
**Fix:** Verify port 10000 is configured
```
Settings ? Environment ? ASPNETCORE_URLS
```

### Issue: Database connection failed
**Fix:** Use Internal Database URL
```
Database ? Connect ? Copy Internal URL
```

### Issue: Build fails
**Fix:** Clear cache and rebuild
```
Manual Deploy ? Clear build cache & deploy
```

## ?? Monitor Your Deployment

### View Logs
```
Dashboard ? clothingeshop-app ? Logs
```

### Check Database
```
Dashboard ? clothingeshop-db ? Info
```

### Test Endpoints
```bash
# Health check
curl https://your-app.onrender.com/health

# List products
curl https://your-app.onrender.com/api/products

# Create product
curl -X POST https://your-app.onrender.com/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Test","description":"Test product","price":9.99,"imageUrl":"https://example.com/img.jpg"}'
```

## ?? Update Your Deployment

### Automatic (Recommended)
Just push to GitHub:
```bash
git add .
git commit -m "Update feature"
git push origin main
```
Render auto-deploys in 2-5 minutes ?

### Manual Deploy
```
Dashboard ? Service ? Manual Deploy ? Deploy latest commit
```

## ?? Cost Breakdown

### Free Tier (What You Get)
- ? Web Service: Free (with sleep)
- ? PostgreSQL: Free for 90 days
- ? 100 GB bandwidth
- ? Automatic SSL
- ? Custom domain support

### Upgrade Options (Optional)
- **Web Service:** $7/month (no sleep, better performance)
- **PostgreSQL:** $7/month (persistent, 1GB storage)

## ?? Post-Deployment Checklist

After deployment completes:

- [ ] Visit app URL and verify it loads
- [ ] Test creating a product
- [ ] Test editing a product
- [ ] Test deleting a product
- [ ] Test API endpoint: `/api/products`
- [ ] Test health endpoint: `/health`
- [ ] Test on mobile device
- [ ] Update README with live URLs
- [ ] Submit URLs for assignment

## ?? Need Help?

### Full Documentation
See [RENDER_DEPLOYMENT.md](RENDER_DEPLOYMENT.md) for:
- Step-by-step screenshots
- Detailed troubleshooting
- Manual setup instructions
- Advanced configuration

### Resources
- **Render Docs:** https://render.com/docs
- **Status Page:** https://status.render.com
- **Community:** https://community.render.com

## ?? Pre-Flight Checklist

Before deploying, ensure:
- [ ] Code is pushed to GitHub
- [ ] `render.yaml` exists
- [ ] `Dockerfile` exposes port 10000
- [ ] No `.env` files committed
- [ ] Application runs locally in Docker
- [ ] All tests pass

## ?? Success Indicators

You'll know deployment succeeded when:
- ? Build completes without errors
- ? Service shows "Live" status (green)
- ? App URL returns your homepage
- ? `/health` returns "Healthy"
- ? API endpoints return data

---

**Estimated Total Time:** 10-15 minutes (including build time)

**First deployment?** It might take up to 15 minutes. Be patient! ?

Good luck! ??
