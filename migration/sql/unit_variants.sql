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

 Date: 10/09/2025 21:24:52
*/


-- ----------------------------
-- Table structure for unit_variants
-- ----------------------------
DROP TABLE IF EXISTS "public"."unit_variants";
CREATE TABLE "public"."unit_variants" (
  "id" int4 NOT NULL DEFAULT nextval('unit_variants_id_seq'::regclass),
  "item_id" int4 NOT NULL,
  "unit_id" int4 NOT NULL,
  "conversion" int4 NOT NULL,
  "sell_price" numeric(12,2) NOT NULL,
  "is_base_unit" bool DEFAULT false,
  "barcode_suffix" text COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Checks structure for table unit_variants
-- ----------------------------
ALTER TABLE "public"."unit_variants" ADD CONSTRAINT "unit_variants_conversion_check" CHECK ((conversion > 0));

-- ----------------------------
-- Primary Key structure for table unit_variants
-- ----------------------------
ALTER TABLE "public"."unit_variants" ADD CONSTRAINT "unit_variants_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table unit_variants
-- ----------------------------
ALTER TABLE "public"."unit_variants" ADD CONSTRAINT "unit_variants_unit_id_fkey" FOREIGN KEY ("unit_id") REFERENCES "public"."units" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
