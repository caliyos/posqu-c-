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

 Date: 10/09/2025 21:29:51
*/


-- ----------------------------
-- Table structure for payment_methods
-- ----------------------------
DROP TABLE IF EXISTS "public"."payment_methods";
CREATE TABLE "public"."payment_methods" (
  "pm_id" int4 NOT NULL DEFAULT nextval('payment_methods_pm_id_seq'::regclass),
  "pm_name" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "pm_number" varchar(50) COLLATE "pg_catalog"."default",
  "pm_note" text COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Primary Key structure for table payment_methods
-- ----------------------------
ALTER TABLE "public"."payment_methods" ADD CONSTRAINT "payment_methods_pkey" PRIMARY KEY ("pm_id");
