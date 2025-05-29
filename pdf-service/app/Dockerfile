# Stage 1: Build Stage
##################################################### 
FROM node:22-slim AS build

WORKDIR /app
RUN npm install -g npm@11.4.1
COPY package*.json ./
RUN npm install
RUN npm ci --include=dev
COPY . ./
RUN npm run build

# Stage 2: Production Stage
#####################################################
FROM node:22-slim AS production

WORKDIR /app
COPY package*.json ./
RUN npm install -g npm@11.4.1 && \
    npm ci --omit=dev && \
    npx playwright install chromium --with-deps

# Tell Playwright where to find the browsers
ENV PLAYWRIGHT_BROWSERS_PATH=/root/.cache/ms-playwright
ENV NODE_ENV=production

# Copy built application and static assets
COPY --from=build /app/dist ./dist
COPY --from=build /app/static ./static

EXPOSE 3000

CMD ["node", "dist/server.js"]