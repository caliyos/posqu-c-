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

 Date: 10/09/2025 21:25:22
*/


-- ----------------------------
-- Table structure for terminals
-- ----------------------------
DROP TABLE IF EXISTS "public"."terminals";
CREATE TABLE "public"."terminals" (
  "id" int4 NOT NULL DEFAULT nextval('terminals_id_seq'::regclass),
  "terminal_name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "pc_id" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "description" text COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Uniques structure for table terminals
-- ----------------------------
ALTER TABLE "public"."terminals" ADD CONSTRAINT "terminals_terminal_name_key" UNIQUE ("terminal_name");

-- ----------------------------
-- Primary Key structure for table terminals
-- ----------------------------
ALTER TABLE "public"."terminals" ADD CONSTRAINT "terminals_pkey" PRIMARY KEY ("id");
