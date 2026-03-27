# ![icon](https://github.com/user-attachments/assets/bfbce3e7-568f-42f6-b8d7-e1af2491705d) 工業 AI 智能助理 ![GitHub watchers](https://img.shields.io/github/watchers/FongYuanChen/IndustrialAICopilot) ![GitHub forks](https://img.shields.io/github/forks/FongYuanChen/IndustrialAICopilot) ![GitHub Repo stars](https://img.shields.io/github/stars/FongYuanChen/IndustrialAICopilot) ![GitHub last commit](https://img.shields.io/github/last-commit/FongYuanChen/IndustrialAICopilot) ![GitHub License](https://img.shields.io/github/license/FongYuanChen/IndustrialAICopilot)

**告別翻找手冊！讓工業 AI 助理幫你解答！**

> 在工業現場，技術手冊與維修日誌往往散落在各處，導致故障排查時需耗費大量時間翻閱文件或依賴特定人員的經驗。本專案致力於改善這個困境，將零散資料整合為企業專屬數位知識庫，並結合 AI 協助現場人員快速定位與解決問題，有效縮短停機時間，並讓關鍵經驗得以持續累積與傳承。

> 本專案旨在實踐 Blazor 與 生成式 AI (RAG) 的技術整合。透過開發這套工業版 AI Copilot，深入探索 LLM 與私有知識庫的對接流程，藉此提升全端開發實力與系統架構設計能力。


## 🎨 功能特色

- **數位化整合**：支援多格式文件導入，自動提取關鍵資訊並建立索引，省去翻閱紙本文件的低效過程。
- **語意化檢索**：支援口語化提問，即便關鍵字不精確，AI 也能直擊文件正確段落。
- **依據式回覆**：回覆內容嚴格鎖定於企業知識庫，確保每條維修建議皆「有據可循」，避免 AI 胡說八道。
- **經驗永續化**：支援匯入維修日誌與現場筆記。讓技術隨案例堆疊持續進化，確保經驗永續傳承。


## 🛠 核心技術

- **語言框架**：基於 C#、.NET 8 與 Blazor Server。
- **資料安全**：使用 [Microsoft.AspNetCore.DataProtection](https://github.com/dotnet/dotnet) 套件加密敏感資訊（如 API Key）。
- **文件解析**：透過 [LangChain.DocumentLoaders](https://github.com/tryAGI/LangChain) 套件支援多格式文件提取。
- **文件分割**：使用 [LangChain.Splitters](https://github.com/tryAGI/LangChain) 套件進行語義片段分割。
- **文字向量化**：透過 [Google.GenAI](https://github.com/googleapis/dotnet-genai) 與 [OpenAI](https://github.com/openai/openai-dotnet/tree/OpenAI_2.9.1) 套件生成文本向量。
- **向量儲存**：使用 [Microsoft.Data.Sqlite](https://github.com/dotnet/dotnet) 套件建立輕量向量資料庫。
- **向量檢索**：自訂 餘弦相似度 (Cosine Similarity) 實作語義搜尋。
- **內容生成**：串接 Google Gemini 與 OpenAI GPT 系列模型，根據檢索結果生成專業維修建議。


## 🖥️ 操作演示

為了方便您快速上手，本專案提供了一份**模擬保養手冊**作為測試資料，您可以下載後上傳至系統建立知識庫：[工業機械手行走軌道 (Linear Track) 定期保養手冊.txt](https://github.com/user-attachments/files/26299138/Linear.Track.txt)



https://github.com/user-attachments/assets/eaef1081-f164-4c04-97c7-430639179f2a


## 📜 授權條款

本專案採用 [MIT](https://github.com/FongYuanChen/IndustrialAICopilot/blob/main/LICENSE) 授權條款。歡迎自由使用、修改與分享！
