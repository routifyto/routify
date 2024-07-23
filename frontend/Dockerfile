# Build stage
FROM node:20-alpine as build

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy all files
COPY . .

# Build the app
RUN npm run build

# Production stage
FROM nginx:alpine

# Copy built assets from build stage
COPY --from=build /app/dist /usr/share/nginx/html

# Add your nginx.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy the shell script
COPY inject-env.sh /docker-entrypoint.d/

# Ensure the script is executable
RUN chmod +x /docker-entrypoint.d/inject-env.sh

# Use the default nginx entrypoint
ENTRYPOINT ["/docker-entrypoint.d/inject-env.sh"]

# Expose port 80
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]