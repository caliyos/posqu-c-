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

 Date: 10/09/2025 21:29:26
*/


-- ----------------------------
-- Table structure for settingtoko
-- ----------------------------
DROP TABLE IF EXISTS "public"."settingtoko";
CREATE TABLE "public"."settingtoko" (
  "id" int4,
  "nama" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "alamat" text COLLATE "pg_catalog"."default" NOT NULL,
  "npwp" varchar(50) COLLATE "pg_catalog"."default",
  "logo" bytea
)
;
