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

 Date: 10/09/2025 21:25:00
*/


-- ----------------------------
-- Table structure for transactions
-- ----------------------------
DROP TABLE IF EXISTS "public"."transactions";
CREATE TABLE "public"."transactions" (
  "ts_id" int4 NOT NULL DEFAULT nextval('transactions_ts_id_seq'::regclass),
  "ts_numbering" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "ts_code" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "ts_total" numeric(18,2) NOT NULL,
  "ts_payment_amount" numeric(18,2) NOT NULL,
  "ts_cashback" numeric(15,2) DEFAULT 0,
  "ts_method" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "ts_status" int2 NOT NULL,
  "ts_change" numeric(15,2) DEFAULT 0,
  "ts_internal_note" text COLLATE "pg_catalog"."default",
  "ts_note" text COLLATE "pg_catalog"."default",
  "ts_customer" int4,
  "ts_freename" varchar(255) COLLATE "pg_catalog"."default",
  "terminal_id" int4,
  "shift_id" int4,
  "user_id" int4,
  "created_by" int4,
  "created_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP,
  "updated_at" timestamp(6) DEFAULT CURRENT_TIMESTAMP,
  "deleted_at" timestamp(6)
)
;

-- ----------------------------
-- Checks structure for table transactions
-- ----------------------------
ALTER TABLE "public"."transactions" ADD CONSTRAINT "transactions_ts_status_check" CHECK ((ts_status = ANY (ARRAY[1, 2, 3])));

-- ----------------------------
-- Primary Key structure for table transactions
-- ----------------------------
ALTER TABLE "public"."transactions" ADD CONSTRAINT "transactions_pkey" PRIMARY KEY ("ts_id");
