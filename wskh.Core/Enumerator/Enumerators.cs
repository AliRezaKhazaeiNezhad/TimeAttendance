using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.Core.Enumerator
{
    /// <summary>
    /// نوع روز کاری
    /// </summary>
    public enum WorkType
    {
        /// <summary>
        /// روزکاری
        /// </summary>
        WorkDay,
        /// <summary>
        /// تعطیلی
        /// </summary>
        Holiday,
        /// <summary>
        /// استراحت
        /// </summary>
        RestDay,
        /// <summary>
        /// تعطیلات و ایام خاص
        /// این مقدار فقط در فرم تعطیلات و ایام خاص استفاده میشود
        /// </summary>
        HolidayAndSpecialDay,
        /// <summary>
        /// غیره
        /// </summary>
        Other
    }


    /// <summary>
    /// نوع زمان
    /// </summary>
    public enum TimeType
    {
        /// <summary>
        /// غیره
        /// </summary>
        Other,
        /// <summary>
        /// زمان کاری
        /// </summary>
        WorkTime,
        /// <summary>
        /// اضافه کار اول وقت
        /// </summary>
        OverTimeStart,
        /// <summary>
        /// اضافه کار آخروقت
        /// </summary>
        OverTimeEnd,
        /// <summary>
        /// نماز / نهار / استراحت
        /// </summary>
        RestTime,
        /// <summary>
        /// عدم حضور
        /// </summary>
        Absence,
        /// <summary>
        /// تردد ناقص
        /// </summary>
        Fraction
    }


    /// <summary>
    /// در صورت عدم ورود و خروج خروج چه كنيم؟
    /// </summary>
    public enum NoExitLog
    {
        /// <summary>
        /// هيچكاري
        /// </summary>
        DoNothing,
        /// <summary>
        /// بستن طبق ساعت كاركرد روز
        /// </summary>
        CloseDailyLeave,
        /// <summary>
        /// بستن تا انتهاي شب
        /// </summary>
        CloseTillNight,
        /// <summary>
        /// بستن در شروع روز بعدي
        /// </summary>
        CloseNextEnterance
    }


    /// <summary>
    /// برنامه کاری
    /// </summary>
    public enum  WorkProgramType
    {
        /// <summary>
        /// نمایش همه
        /// </summary>
        All,
        /// <summary>
        /// برنامه کاری منظم وابسته به روز های هفته
        /// </summary>
        Ordinary,
        /// <summary>
        /// شناور
        /// </summary>
        Flow,
        /// <summary>
        /// برنامه کاری منظم اما بدون وابستگی به روزهای هفته مثل بیمارستانی، نگهبانی و ...
        /// </summary>
        Complex
    }


    /// <summary>
    ///  درصورت تاخیر بیشتر از حد مجاز از ابتدای وقت، چه اتفاقی بیافتد
    /// </summary>
    public enum DelayAction
    {
        /// <summary>
        /// هیچ عملیاتی انجام نشود
        /// </summary>
        Nothing,
        /// <summary>
        /// کسر از استحقاقی- بطور مثال اگر ساعت 8 ورود و تا 8 و پنج دقیقه فرجه باشد، وفرد 8 و ده دقیقه بیاید، باید 10 دقیقه استحقاقی رد شود
        /// </summary>
        DelayFromStart,
        /// <summary>
        /// کسر از استحقاقی- بطور مثال اگر ساعت 8 ورود و تا 8 و پنج دقیقه فرجه باشد، وفرد 8 و ده دقیقه بیاید، باید 5 دقیقه استحقاقی رد شود
        /// </summary>
        DelayRemainTime,
        /// <summary>
        /// غیبت بخورد
        /// </summary>
        Absence,
        /// <summary>
        /// کسر
        /// </summary>
        Disapprove
    }


    /// <summary>
    /// وضعیت ورود و خروج
    /// </summary>
    public enum LogTransportType
    {
        /// <summary>
        /// در انتظار آنالیز
        /// </summary>
        Pending,
        /// <summary>
        /// ورود
        /// </summary>
        Enterance,
        /// <summary>
        /// خروج
        /// </summary>
        Exit
    }


    /// <summary>
    /// کلیدهای تابعی
    /// </summary>
    public enum FunctionKeyType
    {
        /// <summary>
        /// ورود
        /// </summary>
        Enterance,
        /// <summary>
        /// خروج
        /// </summary>
        Exit,
        /// <summary>
        /// مرخصی ساعتی
        /// </summary>
        HourlyLeave,
        /// <summary>
        /// ماموریت ساعتی
        /// </summary>
        HourlyMission,
        /// <summary>
        /// تاخیر سرویس
        /// </summary>
        TransportDelay,
        /// <summary>
        /// نماز، نهار، استراحت
        /// </summary>
        Rest,
        /// <summary>
        /// پاس شیر
        /// </summary>
        ChildRest,
        /// <summary>
        /// غیره
        /// </summary>
        Other,
    }


    /// <summary>
    /// روزهای خاص
    /// </summary>
    public enum SpecialDayType
    {
        /// <summary>
        /// تعطیلی
        /// </summary>
        Holiday,
        /// <summary>
        /// روز خاص
        /// </summary>
        SpecialDay,
        /// <summary>
        /// ایام خاص
        /// </summary>
        SpecialDuration
    }

    /// <summary>
    /// روز هفته
    /// </summary>
    public enum DayInWeek
    {
        /// <summary>
        /// شنبه
        /// </summary>
        Saturday,
        /// <summary>
        /// یکشنبه
        /// </summary>
        Sunday,
        /// <summary>
        /// دوشنبه
        /// </summary>
        Monday,
        /// <summary>
        /// سشنبه
        /// </summary>
        Tuesday,
        /// <summary>
        /// چهارشنبه
        /// </summary>
        Wednesday,
        /// <summary>
        /// پنجشنبه
        /// </summary>
        Thursday,
        /// <summary>
        /// جمعه
        /// </summary>
        Friday
    }


    /// <summary>
    /// وضعیت گزارش
    /// </summary>
    public enum ReportState
    {
        /// <summary>
        /// درحال تحلیل
        /// </summary>
        Analyzing,
        /// <summary>
        /// تردد ناقص
        /// </summary>
        FractionTrade,
        /// <summary>
        /// تحلیل شده
        /// </summary>
        Analyzed
    }


    /// <summary>
    /// وضعیت ورود و خروج
    /// </summary>
    public enum LogState
    {
        /// <summary>
        /// درحال تحلیل
        /// </summary>
        Analyzing,
        /// <summary>
        /// تردد ناقص
        /// </summary>
        FractionTrade,
        /// <summary>
        /// تحلیل شده
        /// </summary>
        Analyzed
    }


    /// <summary>
    /// وضعیت درخواست
    /// </summary>
    public enum RequestState
    {
        /// <summary>
        /// معلق - در انتظار بررسی
        /// </summary>
        Pending,
        /// <summary>
        /// تایید شده است
        /// </summary>
        Approved,
        /// <summary>
        /// رد شده
        /// </summary>
        Rejected,
        /// <summary>
        /// جهت فیلتر همگی
        /// </summary>
        All,
    }


    /// <summary>
    /// نوع درخواست
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// مرخصی ساعتی
        /// </summary>
        HourlyRest,
        /// <summary>
        /// مرخصی روزانه
        /// </summary>
        DailyRest,
        /// <summary>
        /// ماموریت روزانه
        /// </summary>
        MissionDaily,
        /// <summary>
        /// جهت فیلتر همگی
        /// </summary>
        All,
        /// <summary>
        /// ماموریت ساعتی
        /// </summary>
        MissionHourly,
    }



    /// <summary>
    /// وضعیت تیکت
    /// </summary>
    public enum TicketState
    {
        /// <summary>
        /// در انتظار بررسی مدی
        /// </summary>
        Pending,
        /// <summary>
        /// پاسخ داده شده است و بسته شده است
        /// </summary>
        Responsed,
    }


    /// <summary>
    /// وضعیت تردد
    /// </summary>
    public enum TradeType
    {
        /// <summary>
        /// تردد کامل (حضور)
        /// </summary>
        Completed,
        /// <summary>
        /// تردد ناقص
        /// </summary>
        Fraction,
        /// <summary>
        /// غیبت
        /// </summary>
        Absence,
        /// <summary>
        /// نامشخص
        /// </summary>
        UnKnown
    }

    /// <summary>
    /// وضعیت فرمان ها
    /// </summary>
    public enum CommandState
    {
        /// <summary>
        /// درصف پردازش
        /// </summary>
        Pending,
        /// <summary>
        /// درحال پردازش
        /// </summary>
        Analyzing,
        /// <summary>
        /// پردازش شده
        /// </summary>
        Analyzed,
        /// <summary>
        /// عدم انجام
        /// </summary>
        Fraction
    }

    /// <summary>
    /// گروه فرمان
    /// </summary>
    public enum CommandCategory
    {
        /// <summary>
        /// فرمان های تردد
        /// </summary>
        LogCommand,
        /// <summary>
        /// فرمان های انرول
        /// </summary>
        EnrollCommand,
        /// <summary>
        /// فرمان های دستگاه
        /// </summary>
        DeviceCommand,
        /// <summary>
        /// فرمان های تقویم
        /// </summary>
        CalendarUpdate,
        /// <summary>
        /// ایجاد تقویم
        /// </summary>
        CalendarAdded,
        /// <summary>
        /// تغییر در تقویم
        /// </summary>
        SpecialDayChange,
        /// <summary>
        /// انتساب انرول به کاربر
        /// </summary>
        AssignEnrollToUser,
        /// <summary>
        /// حذف انتساب انرول از کاربر سخت افزار
        /// </summary>
        RemoveEnrollFromUser
    }


    public enum ReportDayStep
    {
        /// <summary>
        /// گزارش روزانه ایجاد شده (در حد گزارش خام تردد) و از این مرحله به بعد اطلاعات دیگر افزوده میشود
        /// </summary>
        WaitingToPreAnalyzing,
        /// <summary>
        /// در انتظار انالیز اولیه 
        /// </summary>
        WaitingToFirstAnalyze,
        /// <summary>
        /// آنالیز دوم براساس قوانین و برنامه
        /// </summary>
        WaitingToSecondAnalyze,
        /// <summary>
        /// XXXXXXXXXXXX
        /// </summary>
        XXXXXXX,
    }


    public enum AnalyzedReportState
    {
        /// <summary>
        /// درانتظار انالیز
        /// </summary>
        WaitingForAnalyzing,
        /// <summary>
        /// حاضر در روزکاری
        /// </summary>
        Presence,
        /// <summary>
        /// غایب در روزکاری
        /// عدم ثبت تردد
        /// </summary>
        Absence_NoTransaction,
        /// <summary>
        /// غایب در روزکاری
        /// براساس برنامه کاری غایب است
        /// </summary>
        Absence_Systemic,
        /// <summary>
        ///  روزکاری - تردد ناقص
        /// </summary>
        FractionTrade,

        /// از این بخش به بعد وضعیت را در روز تعطیلی، استراحت، و تعطیلات تقویمی بررسی میکند
        ///درصورتیکه در روز استراحت یا تعطیلی طبق برنامه کاری موظفی نداشته باشد میشود روز استراحت یا تعطیل
        ///درصورتیکه برای روز استراحت یا تعطیل(جمعه) موظفی داشته باشد باید آنالیز و یکی از موارد اضافه کار روز تعطیل، تردد ناقص روز تعطیل، غیبت روز تعطیل یا غیبت سیستمی روز تعطیل میشود

        /// <summary>
        ///روز استراحت
        /// </summary>
        RestDay,
        /// <summary>
        /// روز تعطیل (جمعه) یا تعطیلی رسمی
        /// </summary>
        Holiday,
        /// <summary>
        /// حضور در روز تعطیل
        /// اضافه کار روز تعطیل
        /// </summary>
        Holiday_Presence,
        /// <summary>
        /// تردد ناقص روز تعطیل
        /// </summary>
        HolidayFraction,
        /// <summary>
        /// عدم ثبت تردد
        /// </summary>
        Holiday_Absence_NoTransaction,
        /// <summary>
        /// براساس برنامه کاری غایب است
        /// </summary>
        Holiday_Absence_Systemic,
    }

    /// <summary>
    /// نوع گزارش آنالیز شده را مشخص میکند
    /// </summary>
    public enum AnalyzedReportType
    {
        /// <summary>
        /// چرخشی
        /// </summary>
        Ordinary,
        /// <summary>
        /// شناور
        /// </summary>
        Flow,
        /// <summary>
        /// بیمارستانی
        /// </summary>
        Hospital
    }


    /// <summary>
    /// وضعیت درخواست را مشخص میکند
    /// </summary>
    public enum LeaveState
    {
        /// <summary>
        /// در انتظار بررسی
        /// </summary>
        Pending,
        /// <summary>
        /// تایید شده
        /// </summary>
        Approved,
        /// <summary>
        /// رد شده
        /// </summary>
        Rejected
    }

    /// <summary>
    /// نوع درخواست را مشخص میکند
    /// </summary>
    public enum LeaveType
    {
        /// <summary>
        /// مرخصی روزانه
        /// </summary>
        DailyRst,
        /// <summary>
        /// مرخصی ساعتی
        /// </summary>
        HourlyRest,
        /// <summary>
        /// ماموریت روزانه
        /// </summary>
        DailyMission,
        /// <summary>
        /// ماموریت ساعتی
        /// </summary>
        HourlyMission
    }

}
