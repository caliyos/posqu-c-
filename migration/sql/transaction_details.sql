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

 Date: 10/09/2025 21:25:14
*/


-- ----------------------------
-- Table structure for transaction_details
-- ----------------------------
DROP TABLE IF EXISTS "public"."transaction_details";
CREATE TABLE "public"."transaction_details" (
  "tsd_id" int4 NOT NULL DEFAULT nextval('transaction_details_tsd_id_seq'::regclass),
  "ts_id" int4 NOT NULL,
  "item_id" int4 NOT NULL,
  "tsd_barcode" text COLLATE "pg_catalog"."default",
  "tsd_quantity" numeric(18,2) NOT NULL,
  "tsd_unit" text COLLATE "pg_catalog"."default" NOT NULL,
  "tsd_price_per_unit" numeric(18,2) DEFAULT 0,
  "tsd_unit_variant" text COLLATE "pg_catalog"."default",
  "tsd_conversion_rate" numeric(12,4) DEFAULT 1,
  "tsd_sell_price" numeric(18,2) DEFAULT 0,
  "tsd_total" numeric(20,2) DEFAULT 0,
  "tsd_note" text COLLATE "pg_catalog"."default",
  "tsd_discount_per_item" numeric(15,2) DEFAULT 0,
  "tsd_discount_percentage" numeric(5,2) DEFAULT 0,
  "tsd_discount_total" numeric(18,2) DEFAULT 0,
  "tsd_tax" numeric(18,2) DEFAULT 0,
  "created_by" int4,
  "updated_by" int4,
  "deleted_by" int4,
  "created_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP,
  "updated_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP,
  "deleted_at" timestamp(6)
)
;

-- ----------------------------
-- Primary Key structure for table transaction_details
-- ----------------------------
ALTER TABLE "public"."transaction_details" ADD CONSTRAINT "transaction_details_pkey" PRIMARY KEY ("tsd_id");
