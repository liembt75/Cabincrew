using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Utils
{
    public class Utils
    {
        private static char[] tcvnchars = {
    'µ', '¸', '¶', '·', '¹',
    '¨', '»', '¾', '¼', '½', 'Æ',
    '©', 'Ç', 'Ê', 'È', 'É', 'Ë',
    '®', 'Ì', 'Ð', 'Î', 'Ï', 'Ñ',
    'ª', 'Ò', 'Õ', 'Ó', 'Ô', 'Ö',
    '×', 'Ý', 'Ø', 'Ü', 'Þ',
    'ß', 'ã', 'á', 'â', 'ä',
    '«', 'å', 'è', 'æ', 'ç', 'é',
    '¬', 'ê', 'í', 'ë', 'ì', 'î',
    'ï', 'ó', 'ñ', 'ò', 'ô',
    '­', 'õ', 'ø', 'ö', '÷', 'ù',
    'ú', 'ý', 'û', 'ü', 'þ',
    '¡', '¢', '§', '£', '¤', '¥', '¦'
};

        private static char[] unichars = {
    'à', 'á', 'ả', 'ã', 'ạ',
    'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
    'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
    'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
    'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
    'ì', 'í', 'ỉ', 'ĩ', 'ị',
    'ò', 'ó', 'ỏ', 'õ', 'ọ',
    'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
    'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
    'ù', 'ú', 'ủ', 'ũ', 'ụ',
    'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
    'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
    'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư'
};
    private static char[] convertTable;

    static Utils()
    {
        convertTable = new char[256];
        for (int i = 0; i < 256; i++)
            convertTable[i] = (char)i;
        for (int i = 0; i < tcvnchars.Length; i++)
            convertTable[tcvnchars[i]] = unichars[i];
    }

    public static string TCVN3ToUnicode(string value)
    {
        char[] chars = value.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
            if (chars[i] < (char)256)
                chars[i] = convertTable[chars[i]];
        string rstr = new string(chars);
        return rstr;
    }
   public static string tinhthanh(int matinh)
    {
        string chuoikq = "Khác";
        switch (matinh)
        {
            case 36:
                chuoikq = "Hồ Chí Minh";
                break;
            case 3152:
                chuoikq = "Khánh Hòa"; //Nha Trang
                break;
            case 3172:
                chuoikq = "Hà Nội";
                break;
            case 3173:
                chuoikq = "Hà Giang";
                break;
            case 3174:
                chuoikq = "Cao Bằng";
                break;
            case 3175:
                chuoikq = "Bắc Cạn";
                break;
            case 3176:
                chuoikq = "Tuyên Quang";
                break;
            case 3177:
                chuoikq = "Lào Cai";
                break;
            case 3178:
                chuoikq = "Điện Biên";
                break;
            case 3179:
                chuoikq = "Lai Châu";
                break;
            case 3180:
                chuoikq = "Sơn La";
                break;
            case 3181:
                chuoikq = "Yên Bái";
                break;
            case 3182:
                chuoikq = "Hòa Bình";
                break;
            case 3183:
                chuoikq = "Thái Nguyên";
                break;
            case 3184:
                chuoikq = "Lạng Sơn";
                break;
            case 3185:
                chuoikq = "Quảng Ninh";
                break;
            case 3186:
                chuoikq = "Bắc Giang";
                break;
            case 3187:
                chuoikq = "Phú Thọ";
                break;
            case 3188:
                chuoikq = "Vĩnh Phúc";
                break;
            case 3189:
                chuoikq = "Bắc Ninh";
                break;
            case 3190:
                chuoikq = "Hải Dương";
                break;
            case 3191:
                chuoikq = "Hải Phòng";
                break;
            case 3192:
                chuoikq = "Hưng Yên";
                break;
            case 3193:
                chuoikq = "Thái Bình";
                break;
            case 3194:
                chuoikq = "Hà Nam";
                break;
            case 3195:
                chuoikq = "Nam Định";
                break;
            case 3196:
                chuoikq = "Ninh Bình";
                break;
            case 3197:
                chuoikq = "Thanh Hoá";
                break;
            case 3198:
                chuoikq = "Nghệ An";
                break;
            case 3199:
                chuoikq = "Hà Tĩnh";
                break;
            case 3200:
                chuoikq = "Quảng Bình";
                break;
            case 3201:
                chuoikq = "Quảng Trị";
                break;
            case 3202:
                chuoikq = "Thừa Thiên - Huế";
                break;
            case 3203:
                chuoikq = "Đà Nẵng";
                break;
            case 3204:
                chuoikq = "Quảng Nam";
                break;
            case 3205:
                chuoikq = "Quảng Ngãi";
                break;
            case 3206:
                chuoikq = "Bình Định";
                break;
            case 3207:
                chuoikq = "Phú Yên";
                break;
            case 3208:
                chuoikq = "Khánh Hòa";
                break;
            case 3209:
                chuoikq = "Ninh Thuận";
                break;
            case 3210:
                chuoikq = "Bình Thuận";
                break;
            case 3211:
                chuoikq = "Kon Tum";
                break;
            case 3212:
                chuoikq = "Gia Lai";
                break;
            case 3213:
                chuoikq = "Đắc Lắc";
                break;
            case 3214:
                chuoikq = "Đắk Nông";
                break;
            case 3215:
                chuoikq = "Lâm Đồng";
                break;
            case 3216:
                chuoikq = "Bình Phước";
                break;
            case 3217:
                chuoikq = "Tây Ninh";
                break;
            case 3218:
                chuoikq = "Bình Dương";
                break;
            case 3219:
                chuoikq = "Đồng Nai";
                break;
            case 3220:
                chuoikq = "Bà Rịa - Vũng Tàu";
                break;
            case 3221:
                chuoikq = "Long An";
                break;
            case 3222:
                chuoikq = "Tiền Giang";
                break;
            case 3223:
                chuoikq = "Bến Tre";
                break;
            case 3224:
                chuoikq = "Trà Vinh";
                break;
            case 3225:
                chuoikq = "Vĩnh Long";
                break;
            case 3226:
                chuoikq = "Đồng Tháp";
                break;
            case 3227:
                chuoikq = "An Giang";
                break;
            case 3228:
                chuoikq = "Kiên Giang";
                break;
            case 3229:
                chuoikq = "Cần Thơ";
                break;
            case 3230:
                chuoikq = "Hậu Giang";
                break;
            case 3231:
                chuoikq = "Sóc Trăng";
                break;
            case 3232:
                chuoikq = "Bạc Liêu";
                break;
            case 3233:
                chuoikq = "Cà Mau";
                break;
            case 3234:
            case 3235:
                chuoikq = "Hồ Chí Minh";//Gia Định, Sài Gòn
                break;
            case 3236:
                chuoikq = "1Bắc Thái"; //Bắc Thái ==> Không có
                break;
            case 3237:
                chuoikq = "1Bình Trị Thiên";// Bình Trị Thiên ==> Không có
                break;
            case 3238:
                chuoikq = "1Cửu Long";// Cửu LOng ==> Không có
                break;
            case 3239:
                chuoikq = "1Hoàng Liên Sơn";//Hoàng Liên Sơn ==> Không có
                break;
            case 3240:
                chuoikq = "1Hà Bắc";//Hà Bắc ==> Không có
                break;
            case 3241:
                chuoikq = "1Hà Nam Ninh";//Hà Nam Ninh ==> Không có
                break;
            case 3242:
                chuoikq = "1Hà Sơn Bình";//Hà Sơn Bình ==> Không sai
                break;
            case 3243:
                chuoikq = "1Hà Tuyên";//Hà Tuyên
                break;
            case 3244:
                chuoikq = "Hà Tây";
                break;
            case 3245:
                chuoikq = "1Hải Hưng";//Hải Hưng ==> Không có
                break;
            case 3246:
                chuoikq = "1Minh Hải";//Minh Hải ==> Không có
                break;
            case 3247:
                chuoikq = "1Nam Hà";//Nam Hà ==> Không có
                break;
            case 3248:
                chuoikq = "1Nghệ Tĩnh";//Nghệ Tĩnh ==> Không có
                break;
            case 3249:
                chuoikq = "1Nghĩa Bình";//Nghĩa Bình ==> Không sai
                break;
            case 3250:
                chuoikq = "1Quảng Nam - Đà Nẵng";//Quảng Nam - Đà Nẵng
                break;
            case 3251:
                chuoikq = "1Sông Bé";//Sông Bé ==> Không có
                break;
            case 3252:
                chuoikq = "1Sơn Tây";//Sơn Tây ==> Không có
                break;
            case 3253:
                chuoikq = "1Việt Bắc";//Việt Bắc ==> Không có
                break;
            case 3254:
                chuoikq = "1Vĩnh Phú";//Vĩnh Phú ==> Không có
                break;
            case 3264:
                chuoikq = "1HASUDA";
                break;
            case 3268:
                chuoikq = "1KANAGAWA";
                break;
            case 3281:
                chuoikq = "1TOTTORI";
                break;
            case 3284:
                chuoikq = "1YAMAGUCHI";
                break;
            case 3286:
                chuoikq = "1BUSAN";
                break;
            case 3293:
                chuoikq = "1GYEONGGI DO";
                break;
            case 3294:
                chuoikq = "1INCHEON";
                break;
            case 3299:
                chuoikq = "1SEOUL";
                break;
            case 3314:
                chuoikq = "1MATXCOVA";
                break;
            case 3315:
                chuoikq = "1VARNA";
                break;
            case 3316:
                chuoikq = "1Japan";
                break;
            case 3317:
                chuoikq = "1Korea";
                break;
            case 3319:
                chuoikq = "1Bulgaria";
                break;
            case 3320:
                chuoikq = "1CHLB Đức";
                break;
            case 3321:
                chuoikq = "1Trung Quốc";
                break;
            case 3322:
                chuoikq = "1Hongkong";
                break;
            case 3323:
                chuoikq = "1Liên Bang Nga";
                break;
            case 3324:
                chuoikq = "1Tiệp Khắc";
                break;
            case 3554:
                chuoikq = "1Cục cảnh sát ĐKQL cư trú và DL";
                break;
            case 3558:
                chuoikq = "1Cục quản lý XNC";
                break;
            case 374:
                chuoikq = "Khác";
                break;
            default:
                chuoikq = matinh.ToString();
                break;
        }
        return chuoikq;
    }

    }
    
}
