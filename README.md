# Online Course Platform

คำอธิบายสั้น ๆ: โปรเจกต์ตัวอย่างระบบคอร์สออนไลน์ด้วย .NET 9 (Blazor WebAssembly) แบ่งเป็น Client, Server และ Shared สำหรับ DTO/โมเดลร่วม

**คุณสมบัติหลัก**
- แสดงรายวิชาและรายละเอียดคอร์ส
- จัดการการลงทะเบียนและการเรียนของผู้ใช้
- โครงสร้างแยกเป็น Client (UI), Server (API) และ Shared (DTO/Models)

**เทคโนโลยี**
- .NET 9
- Blazor WebAssembly (Client)
- ASP.NET Core (Server)

**วิธีรันโปรเจกต์ (พื้นฐาน)**
1. เปิด Terminal/CMD ที่ root ของ repo
2. รัน Backend (Server):

   `dotnet run --project src/OnlineCoursePlatform.Server --urls "https://localhost:7248;http://localhost:5280"`

3. รัน Frontend (Client):

   `dotnet run --project src/OnlineCoursePlatform.Client --urls "https://localhost:7164;http://localhost:5300"`

4. เปิดเบราว์เซอร์ไปที่ https://localhost:7164 (หรือพอร์ตที่แสดงในคอนโซล)

**รันเทสต์**
- รันทดสอบทั้งหมด: `dotnet test`

**โครงสร้างไฟล์สำคัญ (โดยย่อ)**
- `src/OnlineCoursePlatform.Client/` — ส่วน UI (Blazor)
- `src/OnlineCoursePlatform.Server/` — ส่วน API และโฮสต์
- `src/OnlineCoursePlatform.Shared/` — DTO และโมเดลที่แชร์
- `tests/` — โฟลเดอร์รวม unit/UI tests
