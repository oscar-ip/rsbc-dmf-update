# Stage 1: Build Stage
##################################################### 
FROM node:22-slim AS build

# Set working directory
WORKDIR /app

# update npm
RUN npm install -g npm@11.4.1

# Copy dependency files
COPY package*.json ./

# Install all dependencies (including devDependencies)
RUN npm install

# Install all dependencies
RUN npm ci --include=dev

# Copy the rest of the files
COPY . ./

# Build the application (TypeScript)
RUN npm run build

# Stage 2: Production Stage
#####################################################
FROM node:22-slim AS production

# Set working directory
WORKDIR /app

# Copy only necessary files
COPY package*.json ./

# update npm
# install browser
# Install only production dependencies
RUN npm install -g npm@11.4.1 && \
    npm ci --omit=dev && \
    npx playwright install chromium --with-deps

# Copy built application and static assets
COPY --from=build /app/dist ./dist
COPY --from=build /app/static ./static

# Expose the port
EXPOSE 3000

# Set environment variables
ENV NODE_ENV=production

# Start the server
CMD ["node", "dist/server.js"]