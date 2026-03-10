/*
 Navicat Premium Data Transfer

 Source Server         : postgresql_LOCAL_11
 Source Server Type    : PostgreSQL
 Source Server Version : 110003
 Source Host           : localhost:5433
 Source Catalog        : posqu
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 110003
 File Encoding         : 65001

 Date: 10/09/2025 21:29:19
*/


-- ----------------------------
-- Table structure for struk_setting
-- ----------------------------
DROP TABLE IF EXISTS "public"."struk_setting";
CREATE TABLE "public"."struk_setting" (
  "id" int4 NOT NULL DEFAULT nextval('struk_setting_id_seq'::regclass),
  "judul" text COLLATE "pg_catalog"."default",
  "alamat" text COLLATE "pg_catalog"."default",
  "telepon" text COLLATE "pg_catalog"."default",
  "footer" text COLLATE "pg_catalog"."default",
  "logo" bytea,
  "is_visible_nama_toko" bool DEFAULT true,
  "is_visible_alamat" bool DEFAULT true,
  "is_visible_telepon" bool DEFAULT true,
  "is_visible_footer" bool DEFAULT true,
  "is_visible_logo" bool DEFAULT true,
  "updated_at" timestamp(6) DEFAULT now()
)
;

-- ----------------------------
-- Primary Key structure for table struk_setting
-- ----------------------------
ALTER TABLE "public"."struk_setting" ADD CONSTRAINT "struk_setting_pkey" PRIMARY KEY ("id");
