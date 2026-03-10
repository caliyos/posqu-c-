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

 Date: 10/09/2025 21:29:35
*/


-- ----------------------------
-- Table structure for receipt_logs
-- ----------------------------
DROP TABLE IF EXISTS "public"."receipt_logs";
CREATE TABLE "public"."receipt_logs" (
  "timestamp" timestamp(6),
  "message" text COLLATE "pg_catalog"."default"
)
;
