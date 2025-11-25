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

 Date: 24/11/2025 17:04:07
*/


-- ----------------------------
-- Table structure for ProductCategories
-- ----------------------------
DROP TABLE IF EXISTS "public"."ProductCategories";
CREATE TABLE "public"."ProductCategories" (
  "ProductId" uuid NOT NULL,
  "CategoryId" uuid NOT NULL,
  "IsDeleted" bool NOT NULL
)
;

-- ----------------------------
-- Records of ProductCategories
-- ----------------------------
INSERT INTO "public"."ProductCategories" VALUES ('f927445c-98a5-46ed-5893-08dae9e7b459', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('f927445c-98a5-46ed-5893-08dae9e7b459', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('f927445c-98a5-46ed-5893-08dae9e7b459', '0d8f2c42-10f2-430f-c9cb-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('f927445c-98a5-46ed-5893-08dae9e7b459', '1e398167-1fb7-4aea-893b-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('bcb8cc05-6131-4a0d-5894-08dae9e7b459', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('bcb8cc05-6131-4a0d-5894-08dae9e7b459', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('bcb8cc05-6131-4a0d-5894-08dae9e7b459', '0d8f2c42-10f2-430f-c9cb-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('bcb8cc05-6131-4a0d-5894-08dae9e7b459', '1e398167-1fb7-4aea-893b-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('38522054-2b2d-48da-5895-08dae9e7b459', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('38522054-2b2d-48da-5895-08dae9e7b459', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('38522054-2b2d-48da-5895-08dae9e7b459', '0d8f2c42-10f2-430f-c9cb-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('38522054-2b2d-48da-5895-08dae9e7b459', '1e398167-1fb7-4aea-893b-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('fa4e8dd9-bd75-4001-5896-08dae9e7b459', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('fa4e8dd9-bd75-4001-5896-08dae9e7b459', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('fa4e8dd9-bd75-4001-5896-08dae9e7b459', '08d32988-df0b-414d-893c-08dae9e6b6ce', 't');
INSERT INTO "public"."ProductCategories" VALUES ('fa4e8dd9-bd75-4001-5896-08dae9e7b459', 'ae63ecdd-ef26-4200-151e-08daeb3a6592', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('70af2d3f-4d8c-4321-5897-08dae9e7b459', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('70af2d3f-4d8c-4321-5897-08dae9e7b459', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('70af2d3f-4d8c-4321-5897-08dae9e7b459', '368bc54b-bd6a-4f7e-893f-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('70af2d3f-4d8c-4321-5897-08dae9e7b459', '0eb2f1b8-f93f-4945-8940-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('4917ff5c-8f99-4f00-5898-08dae9e7b459', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('4917ff5c-8f99-4f00-5898-08dae9e7b459', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('4917ff5c-8f99-4f00-5898-08dae9e7b459', '368bc54b-bd6a-4f7e-893f-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('4917ff5c-8f99-4f00-5898-08dae9e7b459', '66cf0874-deb0-4513-8941-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('f6e123e2-93f2-4a51-5899-08dae9e7b459', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('f6e123e2-93f2-4a51-5899-08dae9e7b459', 'b872dba4-2fa9-4b8a-8942-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('e4883794-e88e-430a-589a-08dae9e7b459', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('e4883794-e88e-430a-589a-08dae9e7b459', 'aabfcc0b-412d-457d-8943-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('dcb66c38-7aad-44ca-e9cc-08daea0e1205', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('dcb66c38-7aad-44ca-e9cc-08daea0e1205', 'ab0ebd4e-574b-4206-8945-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('051fa260-f043-48e4-e9cd-08daea0e1205', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('64c33cc7-29d1-4815-e9ce-08daea0e1205', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('5870392c-3165-47be-e9cf-08daea0e1205', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('5870392c-3165-47be-e9cf-08daea0e1205', '0eb2f1b8-f93f-4945-8940-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('6a0a2e7a-03c8-47f9-e9d0-08daea0e1205', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('bc0cfd7b-786e-4f57-e9d1-08daea0e1205', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('bc0cfd7b-786e-4f57-e9d1-08daea0e1205', 'b1811f8a-6aee-48a0-f3ce-08daea10f2ba', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('c70bad1d-20a5-4527-e9d2-08daea0e1205', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('01088f64-86b8-4600-e9d3-08daea0e1205', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('6e524648-7132-4bd4-e504-08daea4af77b', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('6e524648-7132-4bd4-e504-08daea4af77b', 'ab0ebd4e-574b-4206-8945-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('86affcec-7d9e-41cc-3b11-08daea4e4d1f', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('86affcec-7d9e-41cc-3b11-08daea4e4d1f', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('86affcec-7d9e-41cc-3b11-08daea4e4d1f', '0d8f2c42-10f2-430f-c9cb-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('86affcec-7d9e-41cc-3b11-08daea4e4d1f', '1e398167-1fb7-4aea-893b-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('3c3cc283-3ef8-4cfa-3b12-08daea4e4d1f', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('3c3cc283-3ef8-4cfa-3b12-08daea4e4d1f', 'c52740c2-9631-42d9-d0ec-08daea4d0474', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('5cec15e3-9d35-4bf4-32f0-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('5cec15e3-9d35-4bf4-32f0-08daed648aa0', 'c52740c2-9631-42d9-d0ec-08daea4d0474', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('4e0c687c-a853-4756-32f1-08daed648aa0', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('4e0c687c-a853-4756-32f1-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('4e0c687c-a853-4756-32f1-08daed648aa0', 'ae63ecdd-ef26-4200-151e-08daeb3a6592', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('2e505de0-ab38-4978-32f2-08daed648aa0', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('2e505de0-ab38-4978-32f2-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('44134c4a-95c8-4fa1-32f3-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('5023fc58-aee5-489d-32f4-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('457a1194-fc54-49c2-32f5-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('f52e1ab5-dc5e-4f9e-32f6-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('f1f1ef9f-6f59-4d69-32f7-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('193a59c5-7c4c-4609-32f8-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('193a59c5-7c4c-4609-32f8-08daed648aa0', 'bf198a16-577b-4003-d0eb-08daea4d0474', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('8b3832c6-40eb-4b71-32f9-08daed648aa0', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('8b3832c6-40eb-4b71-32f9-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('706a1c7b-d527-402b-32fa-08daed648aa0', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('706a1c7b-d527-402b-32fa-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('706a1c7b-d527-402b-32fa-08daed648aa0', '1e398167-1fb7-4aea-893b-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('273a42bc-1032-448e-32fb-08daed648aa0', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('273a42bc-1032-448e-32fb-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('273a42bc-1032-448e-32fb-08daed648aa0', '0d8f2c42-10f2-430f-c9cb-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('273a42bc-1032-448e-32fb-08daed648aa0', '1e398167-1fb7-4aea-893b-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('6d60a115-4419-47f2-32fc-08daed648aa0', '8014f906-7e26-4117-c9c9-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('6d60a115-4419-47f2-32fc-08daed648aa0', '3af4cc79-983d-44af-c9ca-08dae2664971', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('6d60a115-4419-47f2-32fc-08daed648aa0', '368bc54b-bd6a-4f7e-893f-08dae9e6b6ce', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('6d60a115-4419-47f2-32fc-08daed648aa0', 'c52740c2-9631-42d9-d0ec-08daea4d0474', 'f');
INSERT INTO "public"."ProductCategories" VALUES ('204d4927-badd-463f-50aa-08db3cb7eec4', '3af4cc79-983d-44af-c9ca-08dae2664971', 't');
INSERT INTO "public"."ProductCategories" VALUES ('204d4927-badd-463f-50aa-08db3cb7eec4', '0eb2f1b8-f93f-4945-8940-08dae9e6b6ce', 't');

-- ----------------------------
-- Indexes structure for table ProductCategories
-- ----------------------------
CREATE INDEX "IX_ProductCategories_CategoryId" ON "public"."ProductCategories" USING btree (
  "CategoryId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table ProductCategories
-- ----------------------------
ALTER TABLE "public"."ProductCategories" ADD CONSTRAINT "PK_ProductCategories" PRIMARY KEY ("ProductId", "CategoryId");

-- ----------------------------
-- Foreign Keys structure for table ProductCategories
-- ----------------------------
ALTER TABLE "public"."ProductCategories" ADD CONSTRAINT "FK_ProductCategories_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "public"."Categories" ("Id") ON DELETE CASCADE ON UPDATE NO ACTION;
ALTER TABLE "public"."ProductCategories" ADD CONSTRAINT "FK_ProductCategories_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "public"."Products" ("Id") ON DELETE CASCADE ON UPDATE NO ACTION;
