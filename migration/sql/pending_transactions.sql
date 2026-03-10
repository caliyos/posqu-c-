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

 Date: 10/09/2025 22:50:56
*/


-- ----------------------------
-- Table structure for pending_transactions
-- ----------------------------
DROP TABLE IF EXISTS "public"."pending_transactions";
CREATE TABLE "public"."pending_transactions" (
  "pt_id" int4 NOT NULL DEFAULT nextval('pending_transactions_pt_id_seq'::regclass),
  "terminal_id" int4 NOT NULL,
  "cashier_id" int4 NOT NULL,
  "ts_id" int4,
  "item_id" int4 NOT NULL,
  "barcode" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "quantity" numeric(15,4) NOT NULL,
  "unit" varchar(20) COLLATE "pg_catalog"."default" NOT NULL,
  "sell_price" numeric(15,2) NOT NULL,
  "discount_percentage" numeric(5,2) DEFAULT 0,
  "discount_total" numeric(15,2) DEFAULT 0,
  "tax" numeric(15,2) DEFAULT 0,
  "total" numeric(18,2) NOT NULL,
  "note" text COLLATE "pg_catalog"."default",
  "created_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP,
  "updated_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP,
  "expired_at" timestamp(6) DEFAULT (CURRENT_TIMESTAMP + '00:15:00'::interval)
)
;

-- ----------------------------
-- Uniques structure for table pending_transactions
-- ----------------------------
ALTER TABLE "public"."pending_transactions" ADD CONSTRAINT "unique_terminal_item" UNIQUE ("terminal_id", "item_id", "unit");

-- ----------------------------
-- Checks structure for table pending_transactions
-- ----------------------------
ALTER TABLE "public"."pending_transactions" ADD CONSTRAINT "pending_transactions_quantity_check" CHECK ((quantity > (0)::numeric));

-- ----------------------------
-- Primary Key structure for table pending_transactions
-- ----------------------------
ALTER TABLE "public"."pending_transactions" ADD CONSTRAINT "pending_transactions_pkey" PRIMARY KEY ("pt_id");

-- ----------------------------
-- Foreign Keys structure for table pending_transactions
-- ----------------------------
ALTER TABLE "public"."pending_transactions" ADD CONSTRAINT "pending_transactions_item_id_fkey" FOREIGN KEY ("item_id") REFERENCES "public"."items" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;
