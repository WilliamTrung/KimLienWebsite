/*
 Navicat Premium Dump SQL

 Source Server         : test_kimlien
 Source Server Type    : PostgreSQL
 Source Server Version : 170006 (170006)
 Source Host           : localhost:5432
 Source Catalog        : postgres
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 170006 (170006)
 File Encoding         : 65001

 Date: 24/11/2025 17:04:15
*/


-- ----------------------------
-- Table structure for Products
-- ----------------------------
DROP TABLE IF EXISTS "public"."Products";
CREATE TABLE "public"."Products" (
  "Id" uuid NOT NULL,
  "Name" text COLLATE "pg_catalog"."default" NOT NULL,
  "Description" text COLLATE "pg_catalog"."default" NOT NULL,
  "Pictures" text COLLATE "pg_catalog"."default" NOT NULL,
  "IsDeleted" bool NOT NULL,
  "CreatedDate" timestamptz(6) NOT NULL,
  "ModifiedDate" timestamptz(6) NOT NULL
)
;

-- ----------------------------
-- Records of Products
-- ----------------------------
INSERT INTO "public"."Products" VALUES ('bcb8cc05-6131-4a0d-5894-08dae9e7b459', 'Túi Đựng Tiền Tự Hủy Viettinbank', '<p><strong>K&iacute;ch Cỡ</strong>:</p>
<ul>
<li>20X30</li>
<li>25X35</li>
<li>30x45</li>
<li>35X50</li>
<li>40X60&nbsp;</li>
</ul>
<p><strong>Loại In</strong>:</p>
<ul>
<li>1 M&agrave;u 1 Mặt</li>
<li>2 M&agrave;u 1 Mặt</li>
<li>1 M&agrave;u 2 Mặt</li>
<li>2 M&agrave;u 2 Mặt.</li>
</ul>', 'https://kimlien1808.blob.core.windows.net/products/bcb8cc05-6131-4a0d-5894-08dae9e7b459_1.jpeg,https://kimlien1808.blob.core.windows.net/products/bcb8cc05-6131-4a0d-5894-08dae9e7b459_2.jpeg,https://kimlien1808.blob.core.windows.net/products/bcb8cc05-6131-4a0d-5894-08dae9e7b459_3.jpeg,https://kimlien1808.blob.core.windows.net/products/bcb8cc05-6131-4a0d-5894-08dae9e7b459_4.jpeg,https://kimlien1808.blob.core.windows.net/products/bcb8cc05-6131-4a0d-5894-08dae9e7b459_5.jpeg,https://kimlien1808.blob.core.windows.net/products/bcb8cc05-6131-4a0d-5894-08dae9e7b459_6.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:08:27.861983+07');
INSERT INTO "public"."Products" VALUES ('38522054-2b2d-48da-5895-08dae9e7b459', 'Túi Đựng Tiền Tự Hủy BIDV', '<p><strong>K&iacute;ch Cỡ</strong>:&nbsp;</p>
<ul>
<li>20X30</li>
<li>25X35</li>
<li>30x45</li>
<li>35X50</li>
<li>40X60&nbsp;</li>
</ul>
<p><strong>Loại In</strong>:</p>
<ul>
<li>1 M&agrave;u 1 Mặt</li>
<li>2 M&agrave;u 1 Mặt</li>
<li>1 M&agrave;u 2 Mặt</li>
<li>2 M&agrave;u 2 Mặt.</li>
</ul>', 'https://kimlien1808.blob.core.windows.net/products/38522054-2b2d-48da-5895-08dae9e7b459_1.jpeg,https://kimlien1808.blob.core.windows.net/products/38522054-2b2d-48da-5895-08dae9e7b459_2.jpeg,https://kimlien1808.blob.core.windows.net/products/38522054-2b2d-48da-5895-08dae9e7b459_3.jpeg,https://kimlien1808.blob.core.windows.net/products/38522054-2b2d-48da-5895-08dae9e7b459_4.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:08:47.120142+07');
INSERT INTO "public"."Products" VALUES ('fa4e8dd9-bd75-4001-5896-08dae9e7b459', 'Bao Vải Đựng Tiền', '<p>T&ugrave;y V&agrave;o Từng Loại Mệnh Gi&aacute; Tiền M&agrave; Bao Được Ph&acirc;n Loại Th&agrave;nh Nhiều Loại Kh&aacute;c Nhau, &nbsp;Đựng Tiền Polymer.</p>', 'https://kimlien1808.blob.core.windows.net/products/fa4e8dd9-bd75-4001-5896-08dae9e7b459_1.jpeg,https://kimlien1808.blob.core.windows.net/products/fa4e8dd9-bd75-4001-5896-08dae9e7b459_2.jpeg,https://kimlien1808.blob.core.windows.net/products/fa4e8dd9-bd75-4001-5896-08dae9e7b459_3.jpeg,https://kimlien1808.blob.core.windows.net/products/fa4e8dd9-bd75-4001-5896-08dae9e7b459_4.jpeg,https://kimlien1808.blob.core.windows.net/products/fa4e8dd9-bd75-4001-5896-08dae9e7b459_5.jpeg,https://kimlien1808.blob.core.windows.net/products/fa4e8dd9-bd75-4001-5896-08dae9e7b459_6.jpeg,https://kimlien1808.blob.core.windows.net/products/fa4e8dd9-bd75-4001-5896-08dae9e7b459_7.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:00:50.900752+07');
INSERT INTO "public"."Products" VALUES ('4917ff5c-8f99-4f00-5898-08dae9e7b459', 'Tem Vỡ Niêm Phong', '<p>1 Tờ A4 Cho Ra 27 Con Tem.</p>', 'https://kimlien1808.blob.core.windows.net/products/4917ff5c-8f99-4f00-5898-08dae9e7b459_2.jpeg,https://kimlien1808.blob.core.windows.net/products/4917ff5c-8f99-4f00-5898-08dae9e7b459_1.jpeg,https://kimlien1808.blob.core.windows.net/products/4917ff5c-8f99-4f00-5898-08dae9e7b459_3.jpeg,https://kimlien1808.blob.core.windows.net/products/4917ff5c-8f99-4f00-5898-08dae9e7b459_4.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:52.072339+07');
INSERT INTO "public"."Products" VALUES ('f6e123e2-93f2-4a51-5899-08dae9e7b459', 'Băng Dính', '<p>B&oacute; T&eacute;p Tiền Bằng Tay.</p>
<p>Th&agrave;nh Phẩm 1 Bịch c&oacute; 1.000 Tờ.</p>', 'https://kimlien1808.blob.core.windows.net/products/f6e123e2-93f2-4a51-5899-08dae9e7b459_1.jpeg,https://kimlien1808.blob.core.windows.net/products/f6e123e2-93f2-4a51-5899-08dae9e7b459_2.jpeg,https://kimlien1808.blob.core.windows.net/products/f6e123e2-93f2-4a51-5899-08dae9e7b459_3.jpeg,https://kimlien1808.blob.core.windows.net/products/f6e123e2-93f2-4a51-5899-08dae9e7b459_4.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:00:45.606837+07');
INSERT INTO "public"."Products" VALUES ('e4883794-e88e-430a-589a-08dae9e7b459', 'Dây Thun', '<p>Hiệu Thun Đại B&agrave;ng.</p>
<p>Th&agrave;nh Phẩm 1 Bịch Tương Đương 0.5Kg.</p>
<p>D&acirc;y Thun C&oacute; Độ Dẻo, Dai, Chắc Tốt.</p>', 'https://kimlien1808.blob.core.windows.net/products/e4883794-e88e-430a-589a-08dae9e7b459_1.jpeg,https://kimlien1808.blob.core.windows.net/products/e4883794-e88e-430a-589a-08dae9e7b459_2.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:22.81109+07');
INSERT INTO "public"."Products" VALUES ('01088f64-86b8-4600-e9d3-08daea0e1205', 'Kệ Bệt', '<p>Kệ Bệt (Pallet)</p>
<p>K&iacute;ch thước: D&agrave;i 78cm x Rộng 62 cm x Cao 12 cm.</p>
<p>Mặt Kệ L&agrave;m Bằng Sắt 1.2ly.</p>
<p>Khung Kệ L&agrave;m Bằng Sắt Hộp 3x6 Dầy 1.6 Ly.</p>
<p>C&aacute;c Ch&acirc;n Kệ L&agrave;m Bằng Thanh La 5cm, Dầy 5 Ly, Uốn Định H&igrave;nh.</p>', 'https://kimlien1808.blob.core.windows.net/products/01088f64-86b8-4600-e9d3-08daea0e1205_2.jpeg,https://kimlien1808.blob.core.windows.net/products/01088f64-86b8-4600-e9d3-08daea0e1205_3.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:00:41.492406+07');
INSERT INTO "public"."Products" VALUES ('051fa260-f043-48e4-e9cd-08daea0e1205', 'Bìa Bao Sổ Tiết Kiệm', '<p>Nhựa Dẻo PE Trơn Trong, C&oacute; In V&agrave; Kh&ocirc;ng In, Nhận In Theo Y&ecirc;u Cầu.</p>', 'https://kimlien1808.blob.core.windows.net/products/051fa260-f043-48e4-e9cd-08daea0e1205_8.jpeg,https://kimlien1808.blob.core.windows.net/products/051fa260-f043-48e4-e9cd-08daea0e1205_2.jpeg,https://kimlien1808.blob.core.windows.net/products/051fa260-f043-48e4-e9cd-08daea0e1205_3.jpeg,https://kimlien1808.blob.core.windows.net/products/051fa260-f043-48e4-e9cd-08daea0e1205_4.jpeg,https://kimlien1808.blob.core.windows.net/products/051fa260-f043-48e4-e9cd-08daea0e1205_5.jpeg,https://kimlien1808.blob.core.windows.net/products/051fa260-f043-48e4-e9cd-08daea0e1205_6.jpeg,https://kimlien1808.blob.core.windows.net/products/051fa260-f043-48e4-e9cd-08daea0e1205_7.jpeg,https://kimlien1808.blob.core.windows.net/products/051fa260-f043-48e4-e9cd-08daea0e1205_9.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:06.475181+07');
INSERT INTO "public"."Products" VALUES ('64c33cc7-29d1-4815-e9ce-08daea0e1205', 'Sổ Ra Vào Kho', '<p>L&agrave;m Theo Mẫu Nhnn Hoặc Theo Mẫu Kh&aacute;ch H&agrave;ng Y&ecirc;u Cầu.</p>', 'https://kimlien1808.blob.core.windows.net/products/64c33cc7-29d1-4815-e9ce-08daea0e1205_8.jpeg,https://kimlien1808.blob.core.windows.net/products/64c33cc7-29d1-4815-e9ce-08daea0e1205_1.jpeg,https://kimlien1808.blob.core.windows.net/products/64c33cc7-29d1-4815-e9ce-08daea0e1205_2.jpeg,https://kimlien1808.blob.core.windows.net/products/64c33cc7-29d1-4815-e9ce-08daea0e1205_3.jpeg,https://kimlien1808.blob.core.windows.net/products/64c33cc7-29d1-4815-e9ce-08daea0e1205_4.jpeg,https://kimlien1808.blob.core.windows.net/products/64c33cc7-29d1-4815-e9ce-08daea0e1205_5.jpeg,https://kimlien1808.blob.core.windows.net/products/64c33cc7-29d1-4815-e9ce-08daea0e1205_6.jpeg,https://kimlien1808.blob.core.windows.net/products/64c33cc7-29d1-4815-e9ce-08daea0e1205_7.jpeg,https://kimlien1808.blob.core.windows.net/products/64c33cc7-29d1-4815-e9ce-08daea0e1205_9.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:48.049034+07');
INSERT INTO "public"."Products" VALUES ('5870392c-3165-47be-e9cf-08daea0e1205', 'Giấy Niêm Phong', '<p>Giấy Ni&ecirc;m Phong Đầu C&acirc;y Tiền Cho Loại Tiền:</p>
<ul>
<li>10 Ngh&igrave;n</li>
<li>20 Ngh&igrave;n</li>
<li>50 Ngh&igrave;n</li>
<li>100 Ngh&igrave;n</li>
<li>200 Ngh&igrave;n</li>
<li>500 Ngh&igrave;n</li>
</ul>', 'https://kimlien1808.blob.core.windows.net/products/5870392c-3165-47be-e9cf-08daea0e1205_1.jpeg,https://kimlien1808.blob.core.windows.net/products/5870392c-3165-47be-e9cf-08daea0e1205_2.jpeg,https://kimlien1808.blob.core.windows.net/products/5870392c-3165-47be-e9cf-08daea0e1205_3.jpeg,https://kimlien1808.blob.core.windows.net/products/5870392c-3165-47be-e9cf-08daea0e1205_4.jpeg,https://kimlien1808.blob.core.windows.net/products/5870392c-3165-47be-e9cf-08daea0e1205_5.jpeg,https://kimlien1808.blob.core.windows.net/products/5870392c-3165-47be-e9cf-08daea0e1205_6.jpeg,https://kimlien1808.blob.core.windows.net/products/5870392c-3165-47be-e9cf-08daea0e1205_7.jpeg,', 't', '2022-11-24 16:51:38+07', '2023-01-04 09:03:43.246229+07');
INSERT INTO "public"."Products" VALUES ('6a0a2e7a-03c8-47f9-e9d0-08daea0e1205', 'Chì Viên Niêm Phong', '<p>D&ugrave;ng Để Ni&ecirc;m Phong, Đ&oacute;ng G&oacute;i Th&agrave;nh Phẩm 1 Bịch = 1Kg (1Kg = 212 Vi&ecirc;n).</p>
<p>(Kh&ocirc;ng bao gồm Kềm)</p>', 'https://kimlien1808.blob.core.windows.net/products/6a0a2e7a-03c8-47f9-e9d0-08daea0e1205_6.jpeg,https://kimlien1808.blob.core.windows.net/products/6a0a2e7a-03c8-47f9-e9d0-08daea0e1205_1.jpeg,https://kimlien1808.blob.core.windows.net/products/6a0a2e7a-03c8-47f9-e9d0-08daea0e1205_2.jpeg,https://kimlien1808.blob.core.windows.net/products/6a0a2e7a-03c8-47f9-e9d0-08daea0e1205_3.jpeg,https://kimlien1808.blob.core.windows.net/products/6a0a2e7a-03c8-47f9-e9d0-08daea0e1205_4.jpeg,https://kimlien1808.blob.core.windows.net/products/6a0a2e7a-03c8-47f9-e9d0-08daea0e1205_5.jpeg,https://kimlien1808.blob.core.windows.net/products/6a0a2e7a-03c8-47f9-e9d0-08daea0e1205_7.jpeg,https://kimlien1808.blob.core.windows.net/products/6a0a2e7a-03c8-47f9-e9d0-08daea0e1205_8.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:18.552149+07');
INSERT INTO "public"."Products" VALUES ('c70bad1d-20a5-4527-e9d2-08daea0e1205', 'Kệ Hồ Sơ', '<p>C&oacute; Nhận L&agrave;m Theo Y&ecirc;u Cầu.</p>
<p>Kệ Sắt Lưu Trữ Hồ Sơ.</p>
<p>Kệ C&oacute; 6 Tầng Gồm 7 Mầm.</p>
<p>Kich Thước: Cao 2m, Rộng 1m, S&acirc;u 0.3m.</p>
<p>Khung Ch&acirc;n Kệ Bằng Sắt V:4cm*6cm, Dầy 1.8mm.</p>
<p>M&acirc;m Kệ Dầy 1mm.</p>', 'https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_9.jpeg,https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_8.jpeg,https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_1.jpeg,https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_2.jpeg,https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_3.jpeg,https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_4.jpeg,https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_5.jpeg,https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_6.jpeg,https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_7.jpeg,https://kimlien1808.blob.core.windows.net/products/c70bad1d-20a5-4527-e9d2-08daea0e1205_10.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:36.359671+07');
INSERT INTO "public"."Products" VALUES ('86affcec-7d9e-41cc-3b11-08daea4e4d1f', 'Túi Đựng Tiền Sacombank', '<p><strong>K&iacute;ch Cỡ</strong>:</p>
<ul>
<li>20X30</li>
<li>25X35</li>
<li>30x45</li>
<li>35X50</li>
<li>40X60&nbsp;</li>
</ul>
<p><strong>Loại In</strong>:</p>
<ul>
<li>1 M&agrave;u 1 Mặt</li>
<li>2 M&agrave;u 1 Mặt</li>
<li>1 M&agrave;u 2 Mặt</li>
<li>2 M&agrave;u 2 Mặt</li>
<li>3 M&agrave;u 2 Mặt</li>
</ul>', 'https://kimlien1808.blob.core.windows.net/products/86affcec-7d9e-41cc-3b11-08daea4e4d1f_1.jpeg,https://kimlien1808.blob.core.windows.net/products/86affcec-7d9e-41cc-3b11-08daea4e4d1f_2.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:10:51.679824+07');
INSERT INTO "public"."Products" VALUES ('3c3cc283-3ef8-4cfa-3b12-08daea4e4d1f', 'Giấy Lót Đầu Cây', '<p>L&agrave; Loại Giấy Ivory 230g, Dẻo Dai, Để L&oacute;t Tiền Mặt Tr&ecirc;n V&agrave; Mặt Dưới.</p>
<p>T&ugrave;y V&agrave;o Mệnh Gi&aacute; Tiền, Giấy L&oacute;t Tiền Được Ph&acirc;n Loại Th&agrave;nh Nhiều Loại Kh&aacute;c Nhau.</p>
<p>Cho C&aacute;c Loại Tiền Từ 500K - 10K (Loại Tiền Polyme).</p>', 'https://kimlien1808.blob.core.windows.net/products/3c3cc283-3ef8-4cfa-3b12-08daea4e4d1f_1.jpeg,https://kimlien1808.blob.core.windows.net/products/3c3cc283-3ef8-4cfa-3b12-08daea4e4d1f_2.jpeg,https://kimlien1808.blob.core.windows.net/products/3c3cc283-3ef8-4cfa-3b12-08daea4e4d1f_3.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:27.498145+07');
INSERT INTO "public"."Products" VALUES ('5cec15e3-9d35-4bf4-32f0-08daed648aa0', 'Băng nhiệt', '<p>L&agrave;m theo thiết kế của ng&acirc;n h&agrave;ng y&ecirc;u cầu.</p>', 'https://kimlien1808.blob.core.windows.net/products/5cec15e3-9d35-4bf4-32f0-08daed648aa0_1.jpeg,https://kimlien1808.blob.core.windows.net/products/5cec15e3-9d35-4bf4-32f0-08daed648aa0_2.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:02.831387+07');
INSERT INTO "public"."Products" VALUES ('4e0c687c-a853-4756-32f1-08daed648aa0', 'Bao Chứng Từ', '<p>L&agrave;m theo thiết kế của ng&acirc;n h&agrave;ng y&ecirc;u cầu.</p>', 'https://kimlien1808.blob.core.windows.net/products/4e0c687c-a853-4756-32f1-08daed648aa0_1.jpeg,https://kimlien1808.blob.core.windows.net/products/4e0c687c-a853-4756-32f1-08daed648aa0_2.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:00:59.214962+07');
INSERT INTO "public"."Products" VALUES ('2e505de0-ab38-4978-32f2-08daed648aa0', 'Bao Thư', '<p>L&agrave;m theo thiết kế của ng&acirc;n h&agrave;ng y&ecirc;u cầu.</p>', 'https://kimlien1808.blob.core.windows.net/products/2e505de0-ab38-4978-32f2-08daed648aa0_1.jpeg,https://kimlien1808.blob.core.windows.net/products/2e505de0-ab38-4978-32f2-08daed648aa0_2.jpeg,https://kimlien1808.blob.core.windows.net/products/2e505de0-ab38-4978-32f2-08daed648aa0_3.jpeg,https://kimlien1808.blob.core.windows.net/products/2e505de0-ab38-4978-32f2-08daed648aa0_4.jpeg,https://kimlien1808.blob.core.windows.net/products/2e505de0-ab38-4978-32f2-08daed648aa0_5.jpeg,https://kimlien1808.blob.core.windows.net/products/2e505de0-ab38-4978-32f2-08daed648aa0_6.jpeg,https://kimlien1808.blob.core.windows.net/products/2e505de0-ab38-4978-32f2-08daed648aa0_7.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:00:55.458718+07');
INSERT INTO "public"."Products" VALUES ('44134c4a-95c8-4fa1-32f3-08daed648aa0', 'Bìa Hộp Simili', '<p>L&agrave;m theo thiết kế của ng&acirc;n h&agrave;ng y&ecirc;u cầu.</p>', 'https://kimlien1808.blob.core.windows.net/products/44134c4a-95c8-4fa1-32f3-08daed648aa0_4.jpeg,https://kimlien1808.blob.core.windows.net/products/44134c4a-95c8-4fa1-32f3-08daed648aa0_1.jpeg,https://kimlien1808.blob.core.windows.net/products/44134c4a-95c8-4fa1-32f3-08daed648aa0_2.jpeg,https://kimlien1808.blob.core.windows.net/products/44134c4a-95c8-4fa1-32f3-08daed648aa0_3.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:10.109644+07');
INSERT INTO "public"."Products" VALUES ('5023fc58-aee5-489d-32f4-08daed648aa0', 'Chỉ Cắt', '<p>D&ugrave;ng để cột đầu bao, chứng từ v&agrave; b&oacute; tiền.</p>', 'https://kimlien1808.blob.core.windows.net/products/5023fc58-aee5-489d-32f4-08daed648aa0_2.jpeg,https://kimlien1808.blob.core.windows.net/products/5023fc58-aee5-489d-32f4-08daed648aa0_1.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:14.265327+07');
INSERT INTO "public"."Products" VALUES ('457a1194-fc54-49c2-32f5-08daed648aa0', 'Chỉ Cuộn', '<p>D&ugrave;ng để cột đầu bao, chứng từ v&agrave; b&oacute; tiền.</p>', 'https://kimlien1808.blob.core.windows.net/products/457a1194-fc54-49c2-32f5-08daed648aa0_1.jpeg,https://kimlien1808.blob.core.windows.net/products/457a1194-fc54-49c2-32f5-08daed648aa0_2.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:02:08.259256+07');
INSERT INTO "public"."Products" VALUES ('f52e1ab5-dc5e-4f9e-32f6-08daed648aa0', 'Hộp Đựng Hồ Sơ Có Dây', '<p>Hộp d&ugrave;ng để đựng hồ sơ chứng từ.</p>', 'https://kimlien1808.blob.core.windows.net/products/f52e1ab5-dc5e-4f9e-32f6-08daed648aa0_2.jpeg,https://kimlien1808.blob.core.windows.net/products/f52e1ab5-dc5e-4f9e-32f6-08daed648aa0_1.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:02:03.876072+07');
INSERT INTO "public"."Products" VALUES ('f1f1ef9f-6f59-4d69-32f7-08daed648aa0', 'Kềm niêm phong', '<p>Kềm d&ugrave;ng cho ch&igrave; ni&ecirc;m phong.</p>', 'https://kimlien1808.blob.core.windows.net/products/f1f1ef9f-6f59-4d69-32f7-08daed648aa0_1.jpeg,https://kimlien1808.blob.core.windows.net/products/f1f1ef9f-6f59-4d69-32f7-08daed648aa0_2.jpeg,https://kimlien1808.blob.core.windows.net/products/f1f1ef9f-6f59-4d69-32f7-08daed648aa0_3.jpeg,https://kimlien1808.blob.core.windows.net/products/f1f1ef9f-6f59-4d69-32f7-08daed648aa0_4.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:40.263414+07');
INSERT INTO "public"."Products" VALUES ('193a59c5-7c4c-4609-32f8-08daed648aa0', 'Keo', '<p>Hồ d&aacute;n.</p>', 'https://kimlien1808.blob.core.windows.net/products/193a59c5-7c4c-4609-32f8-08daed648aa0_1.jpeg,https://kimlien1808.blob.core.windows.net/products/193a59c5-7c4c-4609-32f8-08daed648aa0_2.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:02:40.467123+07');
INSERT INTO "public"."Products" VALUES ('8b3832c6-40eb-4b71-32f9-08daed648aa0', 'Ly Giấy', '<p>In theo y&ecirc;u cầu kh&aacute;ch h&agrave;ng.</p>', 'https://kimlien1808.blob.core.windows.net/products/8b3832c6-40eb-4b71-32f9-08daed648aa0_1.jpeg,https://kimlien1808.blob.core.windows.net/products/8b3832c6-40eb-4b71-32f9-08daed648aa0_2.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:44.096528+07');
INSERT INTO "public"."Products" VALUES ('f927445c-98a5-46ed-5893-08dae9e7b459', 'Túi Đựng Tiền Tự Hủy Agribank', '<p><strong>K&iacute;ch Cỡ</strong>:&nbsp;</p>
<ul>
<li>20X30</li>
<li>25X35</li>
<li>30x45</li>
<li>35X50</li>
<li>40X60&nbsp;</li>
</ul>
<p><strong>Loại In</strong>:</p>
<ul>
<li>1 M&agrave;u 1 Mặt</li>
<li>2 M&agrave;u 1 Mặt</li>
<li>1 M&agrave;u 2 Mặt</li>
<li>2 M&agrave;u 2 Mặt</li>
</ul>', 'https://kimlien1808.blob.core.windows.net/products/f927445c-98a5-46ed-5893-08dae9e7b459_4.jpeg,https://kimlien1808.blob.core.windows.net/products/f927445c-98a5-46ed-5893-08dae9e7b459_1.jpeg,https://kimlien1808.blob.core.windows.net/products/f927445c-98a5-46ed-5893-08dae9e7b459_2.jpeg,https://kimlien1808.blob.core.windows.net/products/f927445c-98a5-46ed-5893-08dae9e7b459_3.jpeg,https://kimlien1808.blob.core.windows.net/products/f927445c-98a5-46ed-5893-08dae9e7b459_5.jpeg,https://kimlien1808.blob.core.windows.net/products/f927445c-98a5-46ed-5893-08dae9e7b459_6.jpeg,https://kimlien1808.blob.core.windows.net/products/f927445c-98a5-46ed-5893-08dae9e7b459_7.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:11:29.76904+07');
INSERT INTO "public"."Products" VALUES ('70af2d3f-4d8c-4321-5897-08dae9e7b459', 'Giấy Niêm Phong', '<p><strong>Chất liệu</strong>: Giấy Pluya Mỏng, In 1 M&agrave;u Tr&ecirc;n 1 Mặt, C&oacute; In T&ecirc;n Chi Nh&aacute;nh Hoặc Kh&ocirc;ng In T&ecirc;n Chi Nh&aacute;nh.<br><strong>Cho Loại Tiền</strong>: 10 Ngh&igrave;n, 20 Ngh&igrave;n, 50 Ngh&igrave;n, 100 Ngh&igrave;n, 200 Ngh&igrave;n, 500 Ngh&igrave;n.</p>', 'https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_5.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_3.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_4.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_6.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_7.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_8.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_9.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_10.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_11.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_12.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_13.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_14.jpeg,https://kimlien1808.blob.core.windows.net/products/70af2d3f-4d8c-4321-5897-08dae9e7b459_15.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:31.904515+07');
INSERT INTO "public"."Products" VALUES ('dcb66c38-7aad-44ca-e9cc-08daea0e1205', 'Xe Đẩy Lưới', '<p>C&oacute; nhận l&agrave;m theo KT y&ecirc;u cầu.</p>
<p>Chi tiết kỹ thuật:</p>
<ul>
<li>K&iacute;ch thước: 900x650x530mm (Chưa t&iacute;nh tay cầm v&agrave; b&aacute;nh xe).</li>
<li>Xung quanh xe được bọc lưới lỗ fi16 d&agrave;y 2mm.</li>
<li>Khung bao l&agrave;m bằng sắt V3 d&agrave;y 1.4mm.</li>
<li>Mặt xe đẩy l&agrave;m bằng th&eacute;p d&agrave;y 1mm được gia cố chịu lực bằng sắt V3 ở dưới mặt.</li>
<li>Tay vịn xe đẩy l&agrave;m bằng sắt fi27 d&agrave;y 1.2mm.</li>
<li>Xe c&oacute; cửa mở ph&iacute;a tr&ecirc;n v&agrave; c&oacute; m&oacute;c kh&oacute;a.</li>
<li>Đường k&iacute;nh b&aacute;nh xe: 130mm, chiều cao b&aacute;nh: 178mm.</li>
<li>Trọng lượng b&aacute;nh xoay: 1.38kg/b&aacute;nh, trọng lượng b&aacute;nh cố định: 0.99kg/b&aacute;nh.</li>
</ul>', 'https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_1.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_2.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_3.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_4.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_5.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_6.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_7.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_8.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_9.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_10.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_11.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_12.jpeg,https://kimlien1808.blob.core.windows.net/products/dcb66c38-7aad-44ca-e9cc-08daea0e1205_13.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:02:50.131719+07');
INSERT INTO "public"."Products" VALUES ('bc0cfd7b-786e-4f57-e9d1-08daea0e1205', 'Tủ Đựng Hồ Sơ', '<p>M&atilde; sản phẩm: THSCC-3NK-21.</p>
<div>T&ecirc;n gọi: Tủ hồ sơ chống ch&aacute;y 03 ngăn k&eacute;o.</div>
<div>+ K&iacute;ch thước:</div>
<ul>
<li><span style="font-size: 15pt;">K&iacute;ch thước: 1300 x 720 x 620mm (Chưa t&iacute;nh b&aacute;nh xe).</span></li>
<li>K&iacute;ch thước ngăn k&eacute;o: 395 x 564 x 560mm.</li>
<li>Trong mỗi ngăn k&eacute;o được chia l&agrave;m 2 ngăn chạy dọc theo chiều s&acirc;u của ngăn k&eacute;o.</li>
<li>K&iacute;ch thước mỗi ngăn: 395 x 282 x 560mm.</li>
</ul>
<div>+ Trong mỗi ngăn k&eacute;o c&oacute; c&aacute;c File để chia ngăn.</div>
<div>+ Tủ l&agrave;m bằng 2 lớp vỏ th&eacute;p d&agrave;y 1.2mm giữa l&agrave; khung xương U chắc chắn v&agrave; chống ch&aacute;y bằng sợi thủy tinh.</div>
<div>+ Tủ c&oacute; 03 ngăn k&eacute;o, mỗi ngăn c&oacute; 01 ổ kh&oacute;a ch&igrave;a, trong mỗi ngăn tủ c&oacute; 02 b&ecirc;n mỗi b&ecirc;n c&oacute; 03 tấm ngăn chia File, khoảng c&aacute;ch giữa c&aacute;c r&atilde;nh của File l&agrave; 5cm.</div>
<div>+ Tủ c&oacute; 04 b&aacute;nh xe di chuyển dễ d&agrave;ng.</div>
<div>+ Tủ sơn theo c&ocirc;ng nghệ sơn b&ocirc;ng, sơn m&agrave;u ghi.</div>
<div>+ Sản xuất tại Việt Nam.</div>', 'https://kimlien1808.blob.core.windows.net/products/bc0cfd7b-786e-4f57-e9d1-08daea0e1205_2.jpeg,https://kimlien1808.blob.core.windows.net/products/bc0cfd7b-786e-4f57-e9d1-08daea0e1205_3.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:01:56.30498+07');
INSERT INTO "public"."Products" VALUES ('273a42bc-1032-448e-32fb-08daed648aa0', 'Túi Tự Hủy Vietcombank', '<p><strong>K&iacute;ch Cỡ</strong>:&nbsp;</p>
<ul>
<li>20X30</li>
<li>25X35</li>
<li>30x45</li>
<li>35X50</li>
<li>40X60&nbsp;</li>
</ul>', 'https://kimlien1808.blob.core.windows.net/products/273a42bc-1032-448e-32fb-08daed648aa0_2.jpeg,https://kimlien1808.blob.core.windows.net/products/273a42bc-1032-448e-32fb-08daed648aa0_1.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:05:50.701295+07');
INSERT INTO "public"."Products" VALUES ('6d60a115-4419-47f2-32fc-08daed648aa0', 'Túi Hồ Sơ', '<p>D&ugrave;ng để đựng hồ sơ theo nhu cầu v&agrave; in theo thiết kế kh&aacute;ch h&agrave;ng.</p>', 'https://kimlien1808.blob.core.windows.net/products/6d60a115-4419-47f2-32fc-08daed648aa0_3.jpeg,https://kimlien1808.blob.core.windows.net/products/6d60a115-4419-47f2-32fc-08daed648aa0_5.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:51:47.630344+07');
INSERT INTO "public"."Products" VALUES ('204d4927-badd-463f-50aa-08db3cb7eec4', 'Giấy Niêm Phong Bidv - Mẫu Mới - Logo Mới Năm  2023', 'Được làm bằng giấy chất liệu Puluya mỏng, đóng gói 1 cọc = 1.000  tờ. Giấy màu nền vàng, in chữ đen. Mỗi mệnh giá sẽ có kích thước tương ứng với từng tờ tiền.', 'https://kimlien1808.blob.core.windows.net/products/204d4927-badd-463f-50aa-08db3cb7eec4_1.jpeg,', 't', '2022-11-24 16:51:38+07', '2023-04-14 07:15:01.992375+07');
INSERT INTO "public"."Products" VALUES ('6e524648-7132-4bd4-e504-08daea4af77b', 'Xe Đẩy Nhật', '<p>K&iacute;ch Thước:</p>
<ul>
<li>D&agrave;i 1240 x Rộng 790mm.</li>
</ul>
<p>&nbsp; &nbsp; &nbsp; &nbsp; + Chiều Cao Tay Cầm: 855mm, Tay Cầm Gấp Được.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;- Đường K&iacute;nh B&aacute;nh Xe: 200mm.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;- Chiều Cao B&aacute;nh Xe: 290mm.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;- Tải Trọng: 500kg.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;- Chuy&ecirc;n D&ugrave;ng Đẩy H&agrave;ng V&agrave; Đẩy Tiền.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; + Chiều Cao Tay Cầm: 800mm, Tay Cầm Gấp Được.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;- Đường K&iacute;nh B&aacute;nh Xe: 150mm.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;- Chiều Cao B&aacute;nh Xe: 240mm.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;- Tải Trọng: 400kg.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;- Chuy&ecirc;n D&ugrave;ng Đẩy H&agrave;ng V&agrave; Đẩy Tiền.</p>
<ul>
<li>D&agrave;i 740 x Rộng 480mm.</li>
</ul>
<p>&nbsp; &nbsp; &nbsp; &nbsp; - Chiều Cao Tay Đẩy: 850mm.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; - Đường K&iacute;nh B&aacute;nh Xe: 100mm.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; - Tải Trọng: 150kg.</p>
<ul>
<li>D&agrave;i 920 x Rộng 610mm.</li>
</ul>
<p>&nbsp; &nbsp; &nbsp; &nbsp; - Chiều Cao Tay Đẩy: 870mm.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; - Đường K&iacute;nh B&aacute;nh Xe: 130mm.</p>
<p>&nbsp; &nbsp; &nbsp; &nbsp; - Tải Trọng: 300kg.</p>', 'https://kimlien1808.blob.core.windows.net/products/6e524648-7132-4bd4-e504-08daea4af77b_1.jpeg,https://kimlien1808.blob.core.windows.net/products/6e524648-7132-4bd4-e504-08daea4af77b_2.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-01-04 09:00:22.292623+07');
INSERT INTO "public"."Products" VALUES ('706a1c7b-d527-402b-32fa-08daed648aa0', 'Túi ATM', '<div>T&ugrave;y v&agrave;o đối tượng Ng&acirc;n h&agrave;ng sử dụng m&agrave; ch&uacute;ng t&ocirc;i đặt l&agrave;m h&agrave;ng theo thiết kế y&ecirc;u cầu.</div>', 'https://kimlien1808.blob.core.windows.net/products/706a1c7b-d527-402b-32fa-08daed648aa0_6.jpeg,https://kimlien1808.blob.core.windows.net/products/706a1c7b-d527-402b-32fa-08daed648aa0_1.jpeg,https://kimlien1808.blob.core.windows.net/products/706a1c7b-d527-402b-32fa-08daed648aa0_2.jpeg,https://kimlien1808.blob.core.windows.net/products/706a1c7b-d527-402b-32fa-08daed648aa0_3.jpeg,https://kimlien1808.blob.core.windows.net/products/706a1c7b-d527-402b-32fa-08daed648aa0_4.jpeg,https://kimlien1808.blob.core.windows.net/products/706a1c7b-d527-402b-32fa-08daed648aa0_5.jpeg,https://kimlien1808.blob.core.windows.net/products/706a1c7b-d527-402b-32fa-08daed648aa0_7.jpeg,https://kimlien1808.blob.core.windows.net/products/706a1c7b-d527-402b-32fa-08daed648aa0_8.jpeg,https://kimlien1808.blob.core.windows.net/products/706a1c7b-d527-402b-32fa-08daed648aa0_9.jpeg,', 'f', '2022-11-24 16:51:38+07', '2023-04-14 07:16:33.768214+07');

-- ----------------------------
-- Primary Key structure for table Products
-- ----------------------------
ALTER TABLE "public"."Products" ADD CONSTRAINT "PK_Products" PRIMARY KEY ("Id");
