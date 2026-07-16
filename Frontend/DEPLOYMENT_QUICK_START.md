# CommunicaAI - Render Deployment Quick Start

## 🚀 Deploy Frontend to Render (Static Site)

### Prerequisites
- GitHub repository with your code
- Render account (free tier available)

---

## Quick Deploy Steps

### 1. Build Settings
```yaml
Root Directory: (leave blank or Frontend)
Build Command: npm install && npm run build
Publish Directory: dist/Frontend/browser
Node Version: 20.11.0
```

### 2. Environment Variables
```
NODE_VERSION=20.11.0
```

### 3. Deploy
Click "Create Static Site" and wait 5-10 minutes.

---

## Important Notes

✅ **Publish Directory:** `dist/Frontend/browser` (NOT `dist/Frontend`)  
✅ **Build Output:** Browser-only SPA (no SSR)  
✅ **Free Tier:** Available  
✅ **Auto-Deploy:** On git push  

---

## Local Testing

```bash
# Install dependencies
npm install

# Build production
npm run build

# Test locally
npx serve -s dist/Frontend/browser -l 3000
```

Visit: `http://localhost:3000`

---

## Troubleshooting

### Build Fails
- Check Node version is 20.x
- Verify all dependencies in package.json
- Check build logs in Render dashboard

### Routes 404
- Render Static Sites automatically handle SPA routing
- No additional configuration needed

### API Not Working
- Update frontend environment.production.ts
- Set correct backend API URL
- Check CORS settings on backend

---

## File Structure

```
dist/Frontend/browser/
├── index.html          ← Entry point
├── main-*.js          ← Application code
├── styles-*.css       ← Styles
├── chunk-*.js         ← Lazy-loaded chunks
└── favicon.ico        ← Icon
```

---

## Cost

**Free Tier:**
- Static Site hosting: **$0/month**
- 100GB bandwidth/month
- Global CDN included
- SSL certificate included

---

## Support

- **Render Docs:** https://render.com/docs/static-sites
- **Status:** https://status.render.com/
- **Community:** https://community.render.com/

---

**Last Updated:** July 10, 2026  
**Status:** ✅ SSR Removed - Ready for Static Deployment
