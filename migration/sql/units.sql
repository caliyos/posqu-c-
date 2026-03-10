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

 Date: 10/09/2025 21:24:39
*/


-- ----------------------------
-- Table structure for units
-- ----------------------------
DROP TABLE IF EXISTS "public"."units";
CREATE TABLE "public"."units" (
  "id" int4 NOT NULL DEFAULT nextval('units_id_seq'::regclass),
  "name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "abbr" varchar(10) COLLATE "pg_catalog"."default" NOT NULL,
  "created_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP,
  "updated_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP
)
;

-- ----------------------------
-- Uniques structure for table units
-- ----------------------------
ALTER TABLE "public"."units" ADD CONSTRAINT "units_abbr_key" UNIQUE ("abbr");

-- ----------------------------
-- Primary Key structure for table units
-- ----------------------------
ALTER TABLE "public"."units" ADD CONSTRAINT "units_pkey" PRIMARY KEY ("id");
