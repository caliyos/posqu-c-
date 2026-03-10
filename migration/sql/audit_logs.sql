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

 Date: 20/10/2025 13:54:50
*/


-- ----------------------------
-- Table structure for audit_logs
-- ----------------------------
DROP TABLE IF EXISTS "public"."audit_logs";
CREATE TABLE "public"."audit_logs" (
  "id" int8 NOT NULL DEFAULT nextval('audit_logs_id_seq'::regclass),
  "user_id" int8,
  "action" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "reference_id" int8,
  "reference_table" varchar(50) COLLATE "pg_catalog"."default",
  "description" text COLLATE "pg_catalog"."default",
  "ip_address" varchar(45) COLLATE "pg_catalog"."default",
  "user_agent" text COLLATE "pg_catalog"."default",
  "created_at" timestamp(6) NOT NULL DEFAULT now()
)
;

-- ----------------------------
-- Primary Key structure for table audit_logs
-- ----------------------------
ALTER TABLE "public"."audit_logs" ADD CONSTRAINT "audit_logs_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table audit_logs
-- ----------------------------
ALTER TABLE "public"."audit_logs" ADD CONSTRAINT "audit_logs_user_id_fkey" FOREIGN KEY ("user_id") REFERENCES "public"."users" ("id") ON DELETE SET NULL ON UPDATE NO ACTION;
