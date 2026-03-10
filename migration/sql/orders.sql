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

 Date: 10/09/2025 21:30:07
*/


-- ----------------------------
-- Table structure for orders
-- ----------------------------
DROP TABLE IF EXISTS "public"."orders";
CREATE TABLE "public"."orders" (
  "order_id" int4 NOT NULL DEFAULT nextval('orders_order_id_seq'::regclass),
  "order_number" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "order_code" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "order_total" numeric(18,2) NOT NULL DEFAULT 0,
  "order_status" int2 NOT NULL,
  "payment_method" varchar(50) COLLATE "pg_catalog"."default",
  "delivery_method" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "delivery_time" timestamp(6),
  "order_note" text COLLATE "pg_catalog"."default",
  "customer_id" int4,
  "customer_name" varchar(255) COLLATE "pg_catalog"."default",
  "customer_phone" varchar(50) COLLATE "pg_catalog"."default",
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
-- Checks structure for table orders
-- ----------------------------
ALTER TABLE "public"."orders" ADD CONSTRAINT "orders_order_status_check" CHECK ((order_status = ANY (ARRAY[0, 1, 2, 3])));

-- ----------------------------
-- Primary Key structure for table orders
-- ----------------------------
ALTER TABLE "public"."orders" ADD CONSTRAINT "orders_pkey" PRIMARY KEY ("order_id");

-- ----------------------------
-- Foreign Keys structure for table orders
-- ----------------------------
ALTER TABLE "public"."orders" ADD CONSTRAINT "fk_order_created_by" FOREIGN KEY ("created_by") REFERENCES "public"."users" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."orders" ADD CONSTRAINT "fk_order_user" FOREIGN KEY ("user_id") REFERENCES "public"."users" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
