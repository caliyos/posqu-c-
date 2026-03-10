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

 Date: 10/09/2025 21:30:42
*/


-- ----------------------------
-- Table structure for delivery
-- ----------------------------
DROP TABLE IF EXISTS "public"."delivery";
CREATE TABLE "public"."delivery" (
  "dlv_id" int4 NOT NULL DEFAULT nextval('delivery_dlv_id_seq'::regclass),
  "transaction_id" int4 NOT NULL,
  "amount" numeric(10,2) NOT NULL,
  "dlv_by" int4 NOT NULL,
  "created_by" int4,
  "updated_by" int4,
  "deleted_by" int4,
  "created_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP,
  "updated_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP,
  "deleted_at" timestamp(6)
)
;

-- ----------------------------
-- Primary Key structure for table delivery
-- ----------------------------
ALTER TABLE "public"."delivery" ADD CONSTRAINT "delivery_pkey" PRIMARY KEY ("dlv_id");
