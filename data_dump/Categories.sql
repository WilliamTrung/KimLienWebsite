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

 Date: 24/11/2025 17:03:54
*/


-- ----------------------------
-- Table structure for Categories
-- ----------------------------
DROP TABLE IF EXISTS "public"."Categories";
CREATE TABLE "public"."Categories" (
  "Id" uuid NOT NULL,
  "Name" text COLLATE "pg_catalog"."default" NOT NULL,
  "ParentId" uuid
)
;

-- ----------------------------
-- Records of Categories
-- ----------------------------
INSERT INTO "public"."Categories" VALUES ('8014f906-7e26-4117-c9c9-08dae2664971', 'In Ấn', NULL);
INSERT INTO "public"."Categories" VALUES ('3af4cc79-983d-44af-c9ca-08dae2664971', 'Vật Dụng Ngân Hàng', NULL);
INSERT INTO "public"."Categories" VALUES ('0d8f2c42-10f2-430f-c9cb-08dae2664971', 'Túi tự hủy', NULL);
INSERT INTO "public"."Categories" VALUES ('b5f2d8a7-14f0-4040-c9cc-08dae2664971', 'Kệ Chứng Từ', NULL);
INSERT INTO "public"."Categories" VALUES ('1e398167-1fb7-4aea-893b-08dae9e6b6ce', 'Túi', '8014f906-7e26-4117-c9c9-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('08d32988-df0b-414d-893c-08dae9e6b6ce', 'Bao', '8014f906-7e26-4117-c9c9-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('368bc54b-bd6a-4f7e-893f-08dae9e6b6ce', 'Giấy', '8014f906-7e26-4117-c9c9-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('0eb2f1b8-f93f-4945-8940-08dae9e6b6ce', 'Giấy niêm phong', '3af4cc79-983d-44af-c9ca-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('66cf0874-deb0-4513-8941-08dae9e6b6ce', 'Tem ', '3af4cc79-983d-44af-c9ca-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('b872dba4-2fa9-4b8a-8942-08dae9e6b6ce', 'Băng Dính', '3af4cc79-983d-44af-c9ca-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('aabfcc0b-412d-457d-8943-08dae9e6b6ce', 'Dây Thun', '3af4cc79-983d-44af-c9ca-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('ab0ebd4e-574b-4206-8945-08dae9e6b6ce', 'Xe Đẩy', '3af4cc79-983d-44af-c9ca-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('0c02f68e-e5dc-492f-8946-08dae9e6b6ce', 'Kệ chứng từ', '3af4cc79-983d-44af-c9ca-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('b1811f8a-6aee-48a0-f3ce-08daea10f2ba', 'Tủ', '3af4cc79-983d-44af-c9ca-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('bf198a16-577b-4003-d0eb-08daea4d0474', 'Keo', '3af4cc79-983d-44af-c9ca-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('c52740c2-9631-42d9-d0ec-08daea4d0474', 'Giấy', '3af4cc79-983d-44af-c9ca-08dae2664971');
INSERT INTO "public"."Categories" VALUES ('ae63ecdd-ef26-4200-151e-08daeb3a6592', 'Bao vải', '3af4cc79-983d-44af-c9ca-08dae2664971');

-- ----------------------------
-- Primary Key structure for table Categories
-- ----------------------------
ALTER TABLE "public"."Categories" ADD CONSTRAINT "PK_Categories" PRIMARY KEY ("Id");
