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

 Date: 10/09/2025 21:30:21
*/


-- ----------------------------
-- Table structure for items
-- ----------------------------
DROP TABLE IF EXISTS "public"."items";
CREATE TABLE "public"."items" (
  "id" int8 NOT NULL DEFAULT nextval('items_id_seq1'::regclass),
  "name" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "buy_price" numeric(15,2) NOT NULL,
  "sell_price" numeric(15,2) NOT NULL,
  "barcode" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "stock" float8 NOT NULL DEFAULT 0,
  "reserved_stock" float8 NOT NULL DEFAULT 0,
  "unit" numeric(15,2) NOT NULL,
  "group" int4 NOT NULL,
  "is_inventory_p" varchar(255) COLLATE "pg_catalog"."default",
  "is_changeprice_p" varchar(255) COLLATE "pg_catalog"."default",
  "materials" varchar(255) COLLATE "pg_catalog"."default",
  "note" varchar(255) COLLATE "pg_catalog"."default",
  "picture" varchar(255) COLLATE "pg_catalog"."default",
  "created_at" timestamp(6) DEFAULT now(),
  "updated_at" timestamp(6) DEFAULT now(),
  "deleted_at" timestamp(6),
  "supplier_id" int4,
  "flag" int4
)
;

-- ----------------------------
-- Uniques structure for table items
-- ----------------------------
ALTER TABLE "public"."items" ADD CONSTRAINT "items_barcode_key" UNIQUE ("barcode");

-- ----------------------------
-- Primary Key structure for table items
-- ----------------------------
ALTER TABLE "public"."items" ADD CONSTRAINT "items_pkey" PRIMARY KEY ("id");
