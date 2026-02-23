# ⛷️❄️ Jalles TC Public Web

Välkommen till **Jalles TC Public Web** – en enkel, rak och rolig webbplats för Jalles TC! ⛷️🏃‍♂️❄️

> **Jalles TC Umeå är idrottsförening grundad 1973. Inriktningen är löpning och skidor. Arrangör av Umemaran & Gammliaterrängen**

---

## 🚀 Vad är detta?

Det här är koden för hemsidan till Jalles TC. Här hittar du allt som behövs för att visa klubbinfo, nyheter och annat kul på webben. Inget krångel, bara träningsglädje i snön eller löparspåret! ❄️

🖥️ Backend baseras på **Umbraco 17** och **.NET 10**.

🎨 Frontend baseras på **Vite**, **JavaScript** och **SASS**.

---

## 🗂️ Projektstruktur

```
├── src/
│   ├── Jalles.Web/                      # ASP.NET Core-webbapp (Umbraco CMS)
│   ├── Jalles.Frontend/                 # Frontend (JS/TS, Vite, SASS)
│   ├── Jalles.BackofficeExtensions/     # Backoffice-tillägg
│   ├── Jalles.Core/                     # Gemensam logik & modeller
│   └── ...
├── test/
│   ├── Jalles.Core.Tests/               # Enhetstester för Core
│   ├── Jalles.Web.Tests/                # Enhetstester för Web
│   ├── Jalles.TestHelpers/              # Testhjälpare och utilities
│   └── ...
├── scripts/                             # Små hjälpskript (t.ex. databas, docker)
├── .github/workflows/                   # CI/CD workflows
├── README.md
```

---

## 🛠️ Kom igång

### 1. Klona repot
```bash
git clone <repo-url>
cd jalles-tc-public-web
```

### 2. Installera förutsättningar
- **.NET 10.0 SDK** ([dot.net](https://dot.net/))
- **Node.js** (helst v20+) ([nodejs.org](https://nodejs.org/))
- **Yarn** ([yarnpkg.com](https://yarnpkg.com/))

### 3. Starta backend (Umbraco)
```bash
cd src/Jalles.Web
# Första gången: dotnet restore
# Starta med hot reload:
dotnet watch run
```

### 4. Starta frontend
```bash
cd src/Jalles.Frontend
yarn install  # Endast första gången eller efter ändringar
yarn dev
```

---

## 🧑‍💻 Utvecklingskommandon

### Backend
```bash
dotnet build         # Bygg lösningen
dotnet watch run     # Starta backend med hot reload
dotnet test          # Kör tester
```

### Frontend
```bash
yarn dev             # Utvecklingsserver
yarn build           # Bygg för produktion
yarn lint:js         # Linta JS/TS
yarn lint:css        # Linta CSS/SASS
```

### Backoffice Extensions
```bash
cd src/Jalles.BackofficeExtensions/Client
yarn install
yarn build           # Bygg backoffice-tillägg
yarn watch           # Utvecklingsläge
```

---

## 🤩 Ha kul!

Det här är Jalles TC – vi gillar snö, skidåkning, löpning och glada utvecklare. Koda på och ha kul! ⛷️❄️🏃‍♀️
