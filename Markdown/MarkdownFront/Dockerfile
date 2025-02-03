# Устанавливаем базовый образ Node.js
FROM node:16 as build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем package.json и package-lock.json
COPY package*.json ./



# Копируем весь проект
COPY . .

# Экспонируем порт
EXPOSE 3000

# Старт nginx
CMD ["npm", "start"]
