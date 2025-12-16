using OnlineCoursePlatform.Shared.DTOs;
using OnlineCoursePlatform.Shared.Models;

namespace OnlineCoursePlatform.Server.Services;

public interface ICourseService
{
    Task<PaginatedResponse<CourseDto>> GetCoursesAsync(CourseFilterDto filter);
    Task<CourseDetailDto?> GetCourseByIdAsync(int id);
    Task<List<CourseDto>> GetFeaturedCoursesAsync(int count = 6);
    Task<List<CourseDto>> GetPopularCoursesAsync(int count = 6);
    Task<List<string>> GetCategoriesAsync();
    Task<CourseDto> CreateCourseAsync(CreateCourseDto dto, int instructorId);
    Task<CourseDto?> UpdateCourseAsync(UpdateCourseDto dto);
    Task<bool> DeleteCourseAsync(int id);
}

public class CourseService : ICourseService
{
    private static readonly List<Course> _courses = GenerateSampleCourses();

    public async Task<PaginatedResponse<CourseDto>> GetCoursesAsync(CourseFilterDto filter)
    {
        await Task.Delay(100); // Simulate async operation

        var query = _courses.Where(c => c.IsPublished).AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            var searchLower = filter.SearchTerm.ToLower();
            query = query.Where(c =>
                c.Title.ToLower().Contains(searchLower) ||
                c.Description.ToLower().Contains(searchLower) ||
                c.Tags.Any(t => t.ToLower().Contains(searchLower)));
        }

        if (!string.IsNullOrEmpty(filter.Category))
        {
            query = query.Where(c => c.Category.Equals(filter.Category, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(filter.Level))
        {
            query = query.Where(c => c.Level.Equals(filter.Level, StringComparison.OrdinalIgnoreCase));
        }

        if (filter.IsFree.HasValue)
        {
            query = filter.IsFree.Value
                ? query.Where(c => c.Price == 0)
                : query.Where(c => c.Price > 0);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(c => c.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(c => c.Price <= filter.MaxPrice.Value);
        }

        if (filter.MinRating.HasValue)
        {
            query = query.Where(c => c.Rating >= filter.MinRating.Value);
        }

        // Apply sorting
        query = filter.SortBy?.ToLower() switch
        {
            "popular" => query.OrderByDescending(c => c.EnrollmentCount),
            "rating" => query.OrderByDescending(c => c.Rating),
            "price-low" => query.OrderBy(c => c.Price),
            "price-high" => query.OrderByDescending(c => c.Price),
            "title" => query.OrderBy(c => c.Title),
            _ => query.OrderByDescending(c => c.CreatedAt)
        };

        var totalCount = query.Count();
        var items = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PaginatedResponse<CourseDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<CourseDetailDto?> GetCourseByIdAsync(int id)
    {
        await Task.Delay(50);
        var course = _courses.FirstOrDefault(c => c.Id == id);
        return course == null ? null : MapToDetailDto(course);
    }

    public async Task<List<CourseDto>> GetFeaturedCoursesAsync(int count = 6)
    {
        await Task.Delay(50);
        return _courses
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.Rating)
            .ThenByDescending(c => c.EnrollmentCount)
            .Take(count)
            .Select(MapToDto)
            .ToList();
    }

    public async Task<List<CourseDto>> GetPopularCoursesAsync(int count = 6)
    {
        await Task.Delay(50);
        return _courses
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.EnrollmentCount)
            .Take(count)
            .Select(MapToDto)
            .ToList();
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        await Task.Delay(50);
        return _courses
            .Where(c => c.IsPublished)
            .Select(c => c.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto dto, int instructorId)
    {
        await Task.Delay(100);

        var course = new Course
        {
            Id = _courses.Max(c => c.Id) + 1,
            Title = dto.Title,
            Description = dto.Description,
            ShortDescription = dto.ShortDescription,
            ImageUrl = dto.ImageUrl,
            Price = dto.Price,
            Category = dto.Category,
            Level = dto.Level,
            Tags = dto.Tags,
            Requirements = dto.Requirements,
            WhatYouWillLearn = dto.WhatYouWillLearn,
            InstructorName = "Instructor",
            IsPublished = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _courses.Add(course);
        return MapToDto(course);
    }

    public async Task<CourseDto?> UpdateCourseAsync(UpdateCourseDto dto)
    {
        await Task.Delay(100);

        var course = _courses.FirstOrDefault(c => c.Id == dto.Id);
        if (course == null) return null;

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.ShortDescription = dto.ShortDescription;
        course.ImageUrl = dto.ImageUrl;
        course.Price = dto.Price;
        course.Category = dto.Category;
        course.Level = dto.Level;
        course.Tags = dto.Tags;
        course.Requirements = dto.Requirements;
        course.WhatYouWillLearn = dto.WhatYouWillLearn;
        course.IsPublished = dto.IsPublished;
        course.UpdatedAt = DateTime.UtcNow;

        return MapToDto(course);
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        await Task.Delay(50);
        var course = _courses.FirstOrDefault(c => c.Id == id);
        if (course == null) return false;

        _courses.Remove(course);
        return true;
    }

    private static CourseDto MapToDto(Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Title = course.Title,
            ShortDescription = course.ShortDescription,
            ImageUrl = course.ImageUrl,
            InstructorName = course.InstructorName,
            Price = course.Price,
            Category = course.Category,
            Level = course.Level,
            Rating = course.Rating,
            RatingCount = course.RatingCount,
            EnrollmentCount = course.EnrollmentCount,
            TotalLessons = course.TotalLessons,
            TotalDuration = FormatDuration(course.TotalDuration),
            Tags = course.Tags
        };
    }

    private static CourseDetailDto MapToDetailDto(Course course)
    {
        return new CourseDetailDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            ShortDescription = course.ShortDescription,
            ImageUrl = course.ImageUrl,
            InstructorName = course.InstructorName,
            InstructorAvatar = course.InstructorAvatar,
            Price = course.Price,
            Category = course.Category,
            Level = course.Level,
            Rating = course.Rating,
            RatingCount = course.RatingCount,
            EnrollmentCount = course.EnrollmentCount,
            TotalLessons = course.TotalLessons,
            TotalDuration = FormatDuration(course.TotalDuration),
            Tags = course.Tags,
            Requirements = course.Requirements,
            WhatYouWillLearn = course.WhatYouWillLearn,
            UpdatedAt = course.UpdatedAt,
            Lessons = course.Lessons.Select(l => new LessonDto
            {
                Id = l.Id,
                CourseId = l.CourseId,
                Title = l.Title,
                Description = l.Description,
                Duration = FormatDuration(l.Duration),
                Order = l.Order,
                Type = l.Type.ToString(),
                IsFreePreview = l.IsFreePreview,
                HasQuiz = l.Quiz != null
            }).ToList()
        };
    }

    private static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalHours >= 1)
        {
            return $"{(int)duration.TotalHours}h {duration.Minutes}m";
        }
        return $"{duration.Minutes}m";
    }

    private static List<Course> GenerateSampleCourses()
    {
        return new List<Course>
        {
            new()
            {
                Id = 1,
                Title = "Complete Web Development Bootcamp 2024",
                Description = "เรียนรู้การพัฒนาเว็บไซต์ตั้งแต่เริ่มต้นจนถึงระดับมืออาชีพ ครอบคลุม HTML, CSS, JavaScript, React และ Node.js พร้อมโปรเจกต์จริงมากกว่า 20 โปรเจกต์",
                ShortDescription = "เรียนรู้การพัฒนาเว็บไซต์ครบวงจร HTML, CSS, JavaScript, React, Node.js",
                ImageUrl = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=800",
                InstructorName = "ดร.สมชาย โปรแกรมเมอร์",
                InstructorAvatar = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150",
                Price = 1990,
                Category = "Web Development",
                Level = "Beginner",
                Rating = 4.8,
                RatingCount = 2547,
                EnrollmentCount = 15420,
                TotalLessons = 156,
                TotalDuration = TimeSpan.FromHours(42),
                IsPublished = true,
                Tags = new List<string> { "HTML", "CSS", "JavaScript", "React", "Node.js" },
                Requirements = new List<string>
                {
                    "คอมพิวเตอร์ที่เชื่อมต่ออินเทอร์เน็ต",
                    "ไม่จำเป็นต้องมีประสบการณ์เขียนโปรแกรมมาก่อน",
                    "ความตั้งใจและเวลาในการเรียนรู้"
                },
                WhatYouWillLearn = new List<string>
                {
                    "สร้างเว็บไซต์ด้วย HTML5 และ CSS3",
                    "เขียน JavaScript ตั้งแต่พื้นฐานถึงขั้นสูง",
                    "พัฒนา Single Page Application ด้วย React",
                    "สร้าง REST API ด้วย Node.js และ Express",
                    "เชื่อมต่อกับฐานข้อมูล MongoDB"
                },
                Lessons = GenerateLessons(1, 10)
            },
            new()
            {
                Id = 2,
                Title = "Machine Learning with Python - From Zero to Hero",
                Description = "เริ่มต้นเรียนรู้ Machine Learning ตั้งแต่พื้นฐาน พร้อมทำโปรเจกต์จริงด้วย Python, Scikit-learn, TensorFlow และ Keras",
                ShortDescription = "เรียนรู้ Machine Learning ด้วย Python ตั้งแต่เริ่มต้น",
                ImageUrl = "https://images.unsplash.com/photo-1555949963-aa79dcee981c?w=800",
                InstructorName = "อ.วิชัย AI Expert",
                InstructorAvatar = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150",
                Price = 2490,
                Category = "Data Science",
                Level = "Intermediate",
                Rating = 4.9,
                RatingCount = 1823,
                EnrollmentCount = 8765,
                TotalLessons = 98,
                TotalDuration = TimeSpan.FromHours(28),
                IsPublished = true,
                Tags = new List<string> { "Python", "Machine Learning", "TensorFlow", "Deep Learning" },
                Requirements = new List<string>
                {
                    "ความรู้พื้นฐาน Python",
                    "พื้นฐานคณิตศาสตร์และสถิติ",
                    "คอมพิวเตอร์ที่รองรับการรัน Python"
                },
                WhatYouWillLearn = new List<string>
                {
                    "ทำความเข้าใจหลักการ Machine Learning",
                    "สร้างโมเดล Supervised และ Unsupervised Learning",
                    "พัฒนา Neural Networks ด้วย TensorFlow",
                    "ประยุกต์ใช้ ML กับปัญหาจริง"
                },
                Lessons = GenerateLessons(2, 8)
            },
            new()
            {
                Id = 3,
                Title = "Flutter & Dart - Build iOS and Android Apps",
                Description = "สร้างแอปมือถือ iOS และ Android ด้วย Flutter Framework เรียนรู้ Dart และ Widget ต่างๆ พร้อมทำแอปจริงตั้งแต่เริ่มต้น",
                ShortDescription = "พัฒนาแอปมือถือ Cross-platform ด้วย Flutter",
                ImageUrl = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=800",
                InstructorName = "คุณภัทร Mobile Dev",
                InstructorAvatar = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150",
                Price = 1790,
                Category = "Mobile Development",
                Level = "Beginner",
                Rating = 4.7,
                RatingCount = 1456,
                EnrollmentCount = 6543,
                TotalLessons = 120,
                TotalDuration = TimeSpan.FromHours(35),
                IsPublished = true,
                Tags = new List<string> { "Flutter", "Dart", "iOS", "Android", "Mobile" },
                Requirements = new List<string>
                {
                    "ความรู้พื้นฐานการเขียนโปรแกรม",
                    "คอมพิวเตอร์ที่รองรับ Flutter SDK"
                },
                WhatYouWillLearn = new List<string>
                {
                    "เขียนภาษา Dart",
                    "สร้าง UI ด้วย Flutter Widgets",
                    "จัดการ State Management",
                    "เชื่อมต่อ REST API",
                    "Publish แอปขึ้น App Store และ Play Store"
                },
                Lessons = GenerateLessons(3, 12)
            },
            new()
            {
                Id = 4,
                Title = "AWS Cloud Practitioner - เตรียมสอบ Certification",
                Description = "เตรียมความพร้อมสอบ AWS Certified Cloud Practitioner เรียนรู้ AWS Services ที่จำเป็นและฝึกทำข้อสอบจริง",
                ShortDescription = "เตรียมสอบ AWS Cloud Practitioner Certification",
                ImageUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=800",
                InstructorName = "อ.ประเสริฐ Cloud Architect",
                InstructorAvatar = "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?w=150",
                Price = 990,
                Category = "Cloud Computing",
                Level = "Beginner",
                Rating = 4.6,
                RatingCount = 987,
                EnrollmentCount = 4321,
                TotalLessons = 65,
                TotalDuration = TimeSpan.FromHours(15),
                IsPublished = true,
                Tags = new List<string> { "AWS", "Cloud", "Certification", "DevOps" },
                Requirements = new List<string>
                {
                    "ไม่จำเป็นต้องมีความรู้ AWS มาก่อน",
                    "ความรู้พื้นฐานด้าน IT"
                },
                WhatYouWillLearn = new List<string>
                {
                    "ทำความเข้าใจ AWS Cloud Concepts",
                    "รู้จัก AWS Core Services",
                    "ความปลอดภัยและ Compliance",
                    "การคิดราคาและ Support Plans"
                },
                Lessons = GenerateLessons(4, 6)
            },
            new()
            {
                Id = 5,
                Title = "เริ่มต้น Git และ GitHub สำหรับมือใหม่",
                Description = "เรียนรู้การใช้งาน Git และ GitHub ตั้งแต่พื้นฐาน Version Control, Branching, Merging จนถึงการทำงานร่วมกับทีม",
                ShortDescription = "เรียน Git และ GitHub ฟรี สำหรับผู้เริ่มต้น",
                ImageUrl = "https://images.unsplash.com/photo-1556075798-4825dfaaf498?w=800",
                InstructorName = "คุณณัฐ Developer",
                InstructorAvatar = "https://images.unsplash.com/photo-1506794778202-cad84cf45f1d?w=150",
                Price = 0,
                Category = "Development Tools",
                Level = "Beginner",
                Rating = 4.8,
                RatingCount = 3456,
                EnrollmentCount = 25678,
                TotalLessons = 25,
                TotalDuration = TimeSpan.FromHours(4),
                IsPublished = true,
                Tags = new List<string> { "Git", "GitHub", "Version Control" },
                Requirements = new List<string>
                {
                    "คอมพิวเตอร์ที่ติดตั้ง Git ได้",
                    "ไม่จำเป็นต้องมีความรู้มาก่อน"
                },
                WhatYouWillLearn = new List<string>
                {
                    "หลักการ Version Control",
                    "คำสั่ง Git พื้นฐาน",
                    "การทำงานกับ GitHub",
                    "Branching และ Merging",
                    "Pull Request และ Code Review"
                },
                Lessons = GenerateLessons(5, 5)
            },
            new()
            {
                Id = 6,
                Title = "UI/UX Design Masterclass with Figma",
                Description = "เรียนรู้การออกแบบ UI/UX ด้วย Figma ตั้งแต่พื้นฐาน Design Principles, Prototyping จนถึง Design System",
                ShortDescription = "ออกแบบ UI/UX อย่างมืออาชีพด้วย Figma",
                ImageUrl = "https://images.unsplash.com/photo-1561070791-2526d30994b5?w=800",
                InstructorName = "คุณพิมพ์ UX Designer",
                InstructorAvatar = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=150",
                Price = 1590,
                Category = "Design",
                Level = "Beginner",
                Rating = 4.9,
                RatingCount = 2134,
                EnrollmentCount = 9876,
                TotalLessons = 85,
                TotalDuration = TimeSpan.FromHours(22),
                IsPublished = true,
                Tags = new List<string> { "UI", "UX", "Figma", "Design" },
                Requirements = new List<string>
                {
                    "ไม่จำเป็นต้องมีประสบการณ์ออกแบบมาก่อน",
                    "คอมพิวเตอร์ที่รัน Figma ได้"
                },
                WhatYouWillLearn = new List<string>
                {
                    "หลักการออกแบบ UI/UX",
                    "ใช้งาน Figma อย่างเชี่ยวชาญ",
                    "สร้าง Prototype และ Animation",
                    "สร้าง Design System"
                },
                Lessons = GenerateLessons(6, 8)
            }
        };
    }

    private static List<Lesson> GenerateLessons(int courseId, int moduleCount)
    {
        var lessons = new List<Lesson>();
        var lessonId = courseId * 100;

        for (int module = 1; module <= moduleCount; module++)
        {
            for (int lesson = 1; lesson <= 3; lesson++)
            {
                lessonId++;
                lessons.Add(new Lesson
                {
                    Id = lessonId,
                    CourseId = courseId,
                    Title = $"Module {module} - Lesson {lesson}",
                    Description = $"บทเรียนที่ {lessons.Count + 1} ของคอร์ส",
                    VideoUrl = "https://sample-videos.com/video.mp4",
                    Duration = TimeSpan.FromMinutes(Random.Shared.Next(10, 30)),
                    Order = lessons.Count + 1,
                    Type = LessonType.Video,
                    IsFreePreview = lessons.Count < 2
                });
            }
        }

        return lessons;
    }
}
