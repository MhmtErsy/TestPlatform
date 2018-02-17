# TestPlatform
2018 - Graduation Project

### ÖZET

Geliştirilecek web tabanlı yazılım test platformunda, kullanıcılar yazılımlarını belirli bir zaman dâhilinde bir fiil başkalarına kullandırtarak da sistemlerindeki hataların tespitini sağlayabilecektir. Test edecek kullanıcılar ise testin büyüklüğüne göre belirli bir ücret karşılığında verilen testleri gerçekleştirip para kazanabileceklerdir. 

Web sitelerinin, mobil veya masaüstü uygulamalarının hatalarının tespitini isteyen müşteriler, test edilmesini istedikleri sistemin, tamamlanması gereken sürenin ve yapılması istenen test konusunun kısa bir özetinin bulunduğu iş ilanını yayınlarlar.
Sistemde Test Master, Tester (Sınayan) ve Client (Müşteri), Admin (Yönetici) olmak üzere 4 türlü rol bulunmaktadır. Sitedeki Test Master kişiler müşteriden aldıkları test işlerinin büyüklüğüne göre belirli sayıda Tester’ı test işine alır. Tester kullanıcılarının çaylak, tecrübeli, uzman gibi rütbeleri bulunmaktadır ve zamanla tamamladıkları test işlerinin büyüklüğüne göre rütbeleri artmaktadır.
Kayıt olduktan sonra kendilerine PC, Mobil Cihaz veya IoT araçları tanımlayabilirler. Test Master’lar başvurdukları test iş ilanına kabul edildikten sonra, verilen testin süresi dâhilinde işleme başlarlar. Atanan Test Master’lar atandıkları işe Test Case’ler oluşturup belirli sayıdaki Tester’ları görevlendirirler. Tester’lar istenen adımları yerine getirirler ve karşılaştıkları sonuçları ya da ekran görüntüsü vb. alınması isteniyorsa bunları belirtirler. Tester’lar (Sınayan), testi başarıyla tamamladıktan sonra Test Master’ın onayından geçer ve Test Master bu işi Müşteri (Customer)’ye raporlar. Müşteri de işi onayladıktan sonra ilgili ücreti Test Master ve Tester’ların banka hesabına yatırırlar.

## Sistem Mimarisi
Sistem 4 farklı rol, 1 sistem arayüzü ve 1 veritabanı olmak üzere 6 elemandan oluşmaktadır.
Sistem mimarisinde bulunan bileşenlerin birbirleri ile kurdukları bağlantı Şekilde gösterilmiştir.

![Şekil 1](https://image.ibb.co/jXUpfn/sm.png)

## ![alt text](https://github.com/adam-p/markdown-here/raw/master/src/common/images/icon48.png "Logo Title Text 1") Modüller

Müşteri kayıt olma

Admin tarafından Test Master kaydı

Tester kayıt olma

Müşteri üye giriş

Tester üye giriş

Test Master üye giriş

Admin üye giriş

Müşteri profil bilgileri güncelleme

Tester profil bilgileri güncelleme

Admin profil bilgileri güncelleme

Test Master profil bilgileri güncelleme

Tester cihaz ekleme

Yayında olan ilanları listeleme

Yayında olmayan ilanları listeleme

Müşteri kendine ait ilanları listeleme

Müşteri ilan oluşturma

Müşteri ilan güncelleme

Admin ilan yayınlama 

Test Master ilana başvurma

Müşteri ilana Test Master atama

Test Master işe kullanım senaryosu oluşturma

Test Master kullanım senaryosu güncelleme

Test Master işe Tester atama

Tester kullanım senaryosu cevaplandırma

Test Master Tester cevabını onaylama

Test Master rapor yazma

Müşteri raporu onaylama

Test Master başvuruları görüntüleme

Test Master işi görüntüleme

Tester kullanım senaryosu görüntüleme

Müşteri iş görüntüleme

## Proje, .NET ortamında MVC yapısı ve katmanlı mimari kullanılarak oluşturuldu.
### Veritabanı, Entity Framework - Code First yaklaşımı ile kodlandı.

![Şekil 2](https://image.ibb.co/dicmRS/km.png)

# Ekran Alıntıları

![Şekil 3](https://image.ibb.co/bGHjfn/ss.png)

![Şekil 4](https://image.ibb.co/mFhH0n/1.png)

![Şekil 5](https://image.ibb.co/hcrzD7/2.png)

![Şekil 6](https://image.ibb.co/dVCvmS/3.png)

![Şekil 7](https://image.ibb.co/m8mc0n/4.png)

![Şekil 8](https://image.ibb.co/hYLsY7/5.png)

![Şekil 9](https://image.ibb.co/bWSvmS/6.png)

![Şekil 10](https://image.ibb.co/nR04fn/7.png)

![Şekil 11](https://image.ibb.co/dC4x0n/8.png)

![Şekil 12](https://image.ibb.co/nO486S/9.png)

![Şekil 13](https://image.ibb.co/keUKD7/10.png)

![Şekil 14](https://image.ibb.co/fv0LLn/11.png)
