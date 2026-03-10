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

 Date: 20/10/2025 14:00:18
*/


-- ----------------------------
-- Table structure for groups
-- ----------------------------
DROP TABLE IF EXISTS "public"."groups";
CREATE TABLE "public"."groups" (
  "id" int8 NOT NULL DEFAULT nextval('groups_id_seq'::regclass),
  "groupname" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "groupshortname" varchar(10) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "created_at" timestamp(0),
  "updated_at" timestamp(0)
)
;

-- ----------------------------
-- Records of groups
-- ----------------------------
INSERT INTO "public"."groups" VALUES (1, 'group1', 'grp1', '2024-09-08 14:11:18', '2024-09-08 14:11:18');
INSERT INTO "public"."groups" VALUES (2, 'group2', 'grp2', '2024-09-08 14:11:18', '2024-09-08 14:11:18');

-- ----------------------------
-- Primary Key structure for table groups
-- ----------------------------
ALTER TABLE "public"."groups" ADD CONSTRAINT "groups_pkey" PRIMARY KEY ("id");
