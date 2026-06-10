-- ======================================================
-- 1. 用户表 SYS_User
-- ======================================================
CREATE TABLE `SYS_User` (
    `Code` CHAR(36) NOT NULL COMMENT '主键 UUID',
    `Name` VARCHAR(100) NOT NULL COMMENT '登录账号',
    `FullName` VARCHAR(100) DEFAULT NULL COMMENT '用户全名',
    `Password` VARCHAR(255) DEFAULT NULL COMMENT '密码（加密）',
    `Email` VARCHAR(200) DEFAULT NULL COMMENT '邮箱',
    `Phone` VARCHAR(20) DEFAULT NULL COMMENT '手机号',
    `IsActive` BIT(1) DEFAULT b'1' COMMENT '是否活动',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否启用',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0' COMMENT '软删除',
    `CreateTime` DATETIME DEFAULT NULL COMMENT '创建时间',
    `CreateUserCode` CHAR(36) DEFAULT NULL COMMENT '创建人Code',
    `ModifyTime` DATETIME DEFAULT NULL COMMENT '修改时间',
    `ModifyUserCode` CHAR(36) DEFAULT NULL COMMENT '修改人Code',
    PRIMARY KEY (`Code`),
    KEY `idx_name` (`Name`),
    KEY `idx_isdelete` (`IsDelete`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='用户表';

-- ======================================================
-- 2. 组织架构表 SYS_Org（支持无限级、虚拟组）
-- ======================================================
CREATE TABLE `SYS_Org` (
    `Code` CHAR(36) NOT NULL COMMENT '主键',
    `ParentCode` CHAR(36) DEFAULT NULL COMMENT '父级组织Code',
    `Name` VARCHAR(200) NOT NULL COMMENT '组织名称',
    `OrgType` VARCHAR(50) NOT NULL COMMENT '组织类型：公司、本部、部、课、班、组、虚拟组',
    `Path` VARCHAR(1000) DEFAULT NULL COMMENT '路径（如 /公司/本部/部/课）',
    `Level` INT NOT NULL DEFAULT 0 COMMENT '层级深度',
    `Sort` INT NOT NULL DEFAULT 0 COMMENT '排序号',
    `IsVirtual` BIT(1) NOT NULL DEFAULT b'0' COMMENT '是否虚拟组',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0',
    `CreateTime` DATETIME DEFAULT NULL,
    `CreateUserCode` CHAR(36) DEFAULT NULL,
    `ModifyTime` DATETIME DEFAULT NULL,
    `ModifyUserCode` CHAR(36) DEFAULT NULL,
    PRIMARY KEY (`Code`),
    KEY `idx_parentcode` (`ParentCode`),
    KEY `idx_path` (`Path`(255))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='组织架构表';

-- ======================================================
-- 3. 角色表 SYS_Role（含角色层级，用于汇报关系）
-- ======================================================
CREATE TABLE `SYS_Role` (
    `Code` CHAR(36) NOT NULL COMMENT '主键',
    `Name` VARCHAR(50) NOT NULL COMMENT '角色名称',
    `SuperiorRoleCode` CHAR(36) DEFAULT NULL COMMENT '默认上级角色Code（自引用，用于汇报链）',
    `Description` VARCHAR(500) DEFAULT NULL,
    `Level` INT NOT NULL DEFAULT 0 COMMENT '角色层级数值（数值越小越低）',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0',
    `CreateTime` DATETIME DEFAULT NULL,
    `CreateUserCode` CHAR(36) DEFAULT NULL,
    `ModifyTime` DATETIME DEFAULT NULL,
    `ModifyUserCode` CHAR(36) DEFAULT NULL,
    PRIMARY KEY (`Code`),
    KEY `idx_superior` (`SuperiorRoleCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='角色表';

-- ======================================================
-- 4. 用户角色分配表 SYS_UserRoleOrg（用户-角色-组织）
-- ======================================================
CREATE TABLE `SYS_UserRoleOrg` (
    `Code` CHAR(36) NOT NULL COMMENT '主键',
    `UserCode` CHAR(36) NOT NULL COMMENT '用户Code',
    `RoleCode` CHAR(36) NOT NULL COMMENT '角色Code',
    `OrgCode` CHAR(36) NOT NULL COMMENT '任职组织Code',
    `IsPrimary` BIT(1) NOT NULL DEFAULT b'0' COMMENT '是否主要角色',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0',
    `CreateTime` DATETIME DEFAULT NULL,
    `CreateUserCode` CHAR(36) DEFAULT NULL,
    `ModifyTime` DATETIME DEFAULT NULL,
    `ModifyUserCode` CHAR(36) DEFAULT NULL,
    PRIMARY KEY (`Code`),
    UNIQUE KEY `uk_user_role_org` (`UserCode`, `RoleCode`, `OrgCode`),
    KEY `idx_usercode` (`UserCode`),
    KEY `idx_rolecode` (`RoleCode`),
    KEY `idx_orgcode` (`OrgCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='用户角色分配表';

-- ======================================================
-- 5. 菜单表 SYS_Route（完整字段，支持动态路由）
-- ======================================================
CREATE TABLE `SYS_Route` (
    `Code` CHAR(36) NOT NULL COMMENT '主键',
    `ParentCode` CHAR(36) DEFAULT NULL COMMENT '父菜单Code',
    `Path` VARCHAR(255) NOT NULL COMMENT '路由路径（支持动态参数）',
    `Component` VARCHAR(255) DEFAULT NULL COMMENT '组件路径（如 views/user/index）',
    `Name` VARCHAR(100) DEFAULT NULL COMMENT '菜单名称（用于路由命名）',
    `Redirect` VARCHAR(255) DEFAULT NULL COMMENT '重定向路径',
    `Hidden` BIT(1) NOT NULL DEFAULT b'0' COMMENT '是否隐藏菜单',
    `AlwaysShow` BIT(1) NOT NULL DEFAULT b'0' COMMENT '是否始终显示父菜单',
    `MetaTitle` VARCHAR(100) DEFAULT NULL COMMENT '菜单显示标题（支持国际化）',
    `MetaIcon` VARCHAR(50) DEFAULT NULL COMMENT '菜单图标（Element Plus 图标名）',
    `MetaNoCache` BIT(1) NOT NULL DEFAULT b'0' COMMENT '是否不缓存页面',
    `MetaAffix` BIT(1) NOT NULL DEFAULT b'0' COMMENT '是否固定标签页',
    `MetaActiveMenu` VARCHAR(255) DEFAULT NULL COMMENT '高亮菜单路径',
    `Sort` INT NOT NULL DEFAULT 0 COMMENT '排序号',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0',
    `CreateTime` DATETIME DEFAULT NULL,
    `CreateUserCode` CHAR(36) DEFAULT NULL,
    `ModifyTime` DATETIME DEFAULT NULL,
    `ModifyUserCode` CHAR(36) DEFAULT NULL,
    PRIMARY KEY (`Code`),
    KEY `idx_parentcode` (`ParentCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='菜单表';

-- ======================================================
-- 6. 按钮表 SYS_Button（完整字段，支持动态按钮事件）
-- ======================================================
CREATE TABLE `SYS_Button` (
    `Code` CHAR(36) NOT NULL COMMENT '主键',
    `ButtonKey` VARCHAR(100) NOT NULL COMMENT '按钮唯一标识（如 Add, Edit）',
    `RouteCode` CHAR(36) NOT NULL COMMENT '所属菜单Code',
    `Name` VARCHAR(100) DEFAULT NULL COMMENT '按钮显示文字',
    `Event` VARCHAR(100) DEFAULT NULL COMMENT '前端触发事件名称（如 handleAdd）',
    `StyleType` VARCHAR(20) DEFAULT 'default' COMMENT '按钮样式：primary, success, warning, danger, info',
    `Type` INT NOT NULL DEFAULT 1 COMMENT '按钮类型：1-普通按钮，2-选中时按钮',
    `Icon` VARCHAR(50) DEFAULT NULL COMMENT '按钮图标',
    `Sort` INT DEFAULT 0 COMMENT '排序',
    `IsSystemButton` BIT(1) NOT NULL DEFAULT b'0' COMMENT '是否系统内置按钮',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0',
    `CreateTime` DATETIME DEFAULT NULL,
    `CreateUserCode` CHAR(36) DEFAULT NULL,
    `ModifyTime` DATETIME DEFAULT NULL,
    `ModifyUserCode` CHAR(36) DEFAULT NULL,
    PRIMARY KEY (`Code`),
    UNIQUE KEY `uk_buttonkey_route` (`ButtonKey`, `RouteCode`),
    KEY `idx_routecode` (`RouteCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='按钮表';

-- ======================================================
-- 7. 菜单权限表 SYS_MenuPermission（支持 User, Role, Org 授权）
-- ======================================================
CREATE TABLE `SYS_MenuPermission` (
    `Code` CHAR(36) NOT NULL COMMENT '主键',
    `RouteCode` CHAR(36) NOT NULL COMMENT '菜单Code',
    `SubjectType` VARCHAR(20) NOT NULL COMMENT '授权主体类型：User, Role, Org',
    `SubjectCode` CHAR(36) NOT NULL COMMENT '主体Code（用户Code/角色Code/组织Code）',
    `IsGranted` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否授权',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0',
    `CreateTime` DATETIME DEFAULT NULL,
    `CreateUserCode` CHAR(36) DEFAULT NULL,
    `ModifyTime` DATETIME DEFAULT NULL,
    `ModifyUserCode` CHAR(36) DEFAULT NULL,
    PRIMARY KEY (`Code`),
    UNIQUE KEY `uk_route_subject` (`RouteCode`, `SubjectType`, `SubjectCode`),
    KEY `idx_subject` (`SubjectType`, `SubjectCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='菜单权限表';

-- ======================================================
-- 8. 按钮权限表 SYS_ButtonPermission（支持 User, Role, Org 授权）
-- ======================================================
CREATE TABLE `SYS_ButtonPermission` (
    `Code` CHAR(36) NOT NULL COMMENT '主键',
    `ButtonCode` CHAR(36) NOT NULL COMMENT '按钮Code',
    `SubjectType` VARCHAR(20) NOT NULL COMMENT '授权主体类型：User, Role, Org',
    `SubjectCode` CHAR(36) NOT NULL COMMENT '主体Code',
    `IsGranted` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否授权',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0',
    `CreateTime` DATETIME DEFAULT NULL,
    `CreateUserCode` CHAR(36) DEFAULT NULL,
    `ModifyTime` DATETIME DEFAULT NULL,
    `ModifyUserCode` CHAR(36) DEFAULT NULL,
    PRIMARY KEY (`Code`),
    UNIQUE KEY `uk_button_subject` (`ButtonCode`, `SubjectType`, `SubjectCode`),
    KEY `idx_subject` (`SubjectType`, `SubjectCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='按钮权限表';

-- ======================================================
-- 权限委托表（支持权限下放）
-- ======================================================
CREATE TABLE `SYS_PermissionDelegation` (
    `Code` CHAR(36) NOT NULL COMMENT '主键 UUID',
    `FromUserCode` CHAR(36) NOT NULL COMMENT '授权人（委托方）用户Code',
    `ToUserCode` CHAR(36) NOT NULL COMMENT '被授权人（受托方）用户Code',
    `ButtonCode` CHAR(36) NOT NULL COMMENT '下放的按钮Code（关联 SYS_Button）',
    `RouteCode` CHAR(36) DEFAULT NULL COMMENT '限定菜单Code（可为空，表示该按钮下所有菜单实例）',
    `Condition` JSON DEFAULT NULL COMMENT '附加条件，JSON格式，如 {"DepartmentCode": "xxx", "Amount": {"$lte": 10000}}',
    `EffectiveStartDate` DATETIME NOT NULL COMMENT '生效开始时间',
    `EffectiveEndDate` DATETIME NOT NULL COMMENT '生效结束时间',
    `IsActive` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否启用',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0' COMMENT '软删除',
    `CreateTime` DATETIME DEFAULT NULL,
    `CreateUserCode` CHAR(36) DEFAULT NULL,
    `ModifyTime` DATETIME DEFAULT NULL,
    `ModifyUserCode` CHAR(36) DEFAULT NULL,
    PRIMARY KEY (`Code`),
    KEY `idx_fromuser` (`FromUserCode`),
    KEY `idx_touser` (`ToUserCode`),
    KEY `idx_button` (`ButtonCode`),
    KEY `idx_effectivedate` (`EffectiveStartDate`, `EffectiveEndDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='权限委托表（用于权限下放）';

-- ======================================================
-- 9. 汇报关系指定表 SYS_ReportLine（仅用于打破角色层级）
-- ======================================================
CREATE TABLE `SYS_ReportLine` (
    `Code` CHAR(36) NOT NULL COMMENT '主键',
    `UserCode` CHAR(36) NOT NULL COMMENT '员工用户Code',
    `SupervisorUserCode` CHAR(36) NOT NULL COMMENT '直接上级用户Code',
    `OrgCode` CHAR(36) NOT NULL COMMENT '所在组织Code',
    `RoleCode` CHAR(36) DEFAULT NULL COMMENT '担任的角色Code（冗余）',
    `IsActive` BIT(1) NOT NULL DEFAULT b'1',
    `EffectiveDate` DATE DEFAULT NULL,
    `ExpiryDate` DATE DEFAULT NULL,
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0',
    `CreateTime` DATETIME DEFAULT NULL,
    `CreateUserCode` CHAR(36) DEFAULT NULL,
    `ModifyTime` DATETIME DEFAULT NULL,
    `ModifyUserCode` CHAR(36) DEFAULT NULL,
    PRIMARY KEY (`Code`),
    KEY `idx_usercode` (`UserCode`),
    KEY `idx_supervisor` (`SupervisorUserCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='指定汇报关系表';



-- ======================================================
-- 字典表 SYS_Dictionary
-- ======================================================
CREATE TABLE `SYS_Dictionary` (
    `Code` CHAR(36) NOT NULL COMMENT '主键 UUID',
    `ParentCode` CHAR(36) DEFAULT NULL COMMENT '父级字典Code',
    `Type` VARCHAR(50) DEFAULT NULL COMMENT '字典类别',
    `KeyText` VARCHAR(100) DEFAULT NULL COMMENT '键',
    `ValueText` VARCHAR(500) DEFAULT NULL COMMENT '值',
    `Description` VARCHAR(500) DEFAULT NULL COMMENT '描述',
    `Sort` INT NOT NULL DEFAULT 0 COMMENT '排序',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否启用',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0' COMMENT '软删除',
    `CreateTime` DATETIME DEFAULT NULL COMMENT '创建时间',
    `CreateUserCode` CHAR(36) DEFAULT NULL COMMENT '创建人Code',
    `ModifyTime` DATETIME DEFAULT NULL COMMENT '修改时间',
    `ModifyUserCode` CHAR(36) DEFAULT NULL COMMENT '修改人Code',
    PRIMARY KEY (`Code`),
    KEY `idx_parentcode` (`ParentCode`),
    KEY `idx_type` (`Type`),
    KEY `idx_keytext` (`KeyText`),
    KEY `idx_isdelete` (`IsDelete`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='字典表';

-- ======================================================
-- 附件表 SYS_Attachment
-- ======================================================
CREATE TABLE `SYS_Attachment` (
    `Code` CHAR(36) NOT NULL COMMENT '主键 UUID',
    `FileName` VARCHAR(500) DEFAULT NULL COMMENT '文件名（含扩展名）',
    `FileExtension` VARCHAR(50) DEFAULT NULL COMMENT '文件扩展名',
    `FileClientName` VARCHAR(500) DEFAULT NULL COMMENT '文件原名',
    `FilePath` VARCHAR(1000) DEFAULT NULL COMMENT '完整存储路径（如OSS路径）',
    `FileType` VARCHAR(100) DEFAULT NULL COMMENT '文件MIME类型（如 image/png）',
    `FileSize` BIGINT DEFAULT NULL COMMENT '文件大小（字节）',
    `MD5Hash` VARCHAR(32) DEFAULT NULL COMMENT '文件MD5校验值（32位十六进制）',
    `DownloadCount` INT DEFAULT 0 COMMENT '下载次数',
    `Description` VARCHAR(500) DEFAULT NULL COMMENT '文件描述',
    `EntityType` VARCHAR(100) DEFAULT NULL COMMENT '业务实体类型（如 ApplicationForm, Order）',
    `EntityCode` CHAR(36) NOT NULL COMMENT '关联的业务实体Code',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否启用',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0' COMMENT '软删除',
    `CreateTime` DATETIME DEFAULT NULL COMMENT '创建时间',
    `CreateUserCode` CHAR(36) DEFAULT NULL COMMENT '创建人Code',
    `ModifyTime` DATETIME DEFAULT NULL COMMENT '修改时间',
    `ModifyUserCode` CHAR(36) DEFAULT NULL COMMENT '修改人Code',
    PRIMARY KEY (`Code`),
    KEY `idx_entity` (`EntityType`, `EntityCode`),
    KEY `idx_md5hash` (`MD5Hash`),
    KEY `idx_isdelete` (`IsDelete`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='通用附件表';

-- ======================================================
-- 邮件队列表 SYS_EmailQueue
-- ======================================================
CREATE TABLE `SYS_EmailQueue` (
    `Code` CHAR(36) NOT NULL COMMENT '主键 UUID',
    `EmailCode` VARCHAR(100) NOT NULL COMMENT '邮件编号',
    `Subject` VARCHAR(500) NOT NULL COMMENT '邮件标题',
    `Content` LONGTEXT NOT NULL COMMENT '邮件内容（支持HTML）',
    `ToAddress` VARCHAR(1000) NOT NULL COMMENT '收件人，多个邮箱使用 ; 分隔',
    `CcAddress` VARCHAR(1000) DEFAULT NULL COMMENT '抄送人',
    `BccAddress` VARCHAR(1000) DEFAULT NULL COMMENT '密送人',
    `IsBodyHtml` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否HTML格式',
    `SendStatus` INT NOT NULL DEFAULT 0 COMMENT '发送状态：0待发送 1发送中 2发送成功 3发送失败',
    `RetryCount` INT NOT NULL DEFAULT 0 COMMENT '重试次数',
    `MaxRetryCount` INT NOT NULL DEFAULT 5 COMMENT '最大重试次数',
    `SentTime` DATETIME DEFAULT NULL COMMENT '实际发送时间',
    `ErrorMessage` TEXT DEFAULT NULL COMMENT '错误信息',
    `BusinessType` VARCHAR(100) DEFAULT NULL COMMENT '业务类型',
    `BusinessId` VARCHAR(100) DEFAULT NULL COMMENT '业务ID',
    `Remark` VARCHAR(500) DEFAULT NULL COMMENT '备注',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否启用',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0' COMMENT '软删除',
    `CreateTime` DATETIME DEFAULT NULL COMMENT '创建时间',
    `CreateUserCode` CHAR(36) DEFAULT NULL COMMENT '创建人Code',
    `ModifyTime` DATETIME DEFAULT NULL COMMENT '修改时间',
    `ModifyUserCode` CHAR(36) DEFAULT NULL COMMENT '修改人Code',
    PRIMARY KEY (`Code`),
    KEY `idx_emailcode` (`EmailCode`),
    KEY `idx_sendstatus` (`SendStatus`),
    KEY `idx_businesstype` (`BusinessType`),
    KEY `idx_businessid` (`BusinessId`),
    KEY `idx_createtime` (`CreateTime`),
    KEY `idx_isdelete` (`IsDelete`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='邮件发送队列表';

-- ======================================================
-- 日志表 SYS_Log
-- ======================================================
CREATE TABLE `SYS_Log` (
    `Code` CHAR(36) NOT NULL COMMENT '主键 UUID',
    `UserName` VARCHAR(100) DEFAULT NULL COMMENT '操作用户名',
    `Type` VARCHAR(50) DEFAULT NULL COMMENT '日志类型：Request(网络日志)、System(系统日志)、User(用户操作日志)、Security(安全日志)',
    `MenuName` VARCHAR(200) DEFAULT NULL COMMENT '菜单名称',
    `ModuleName` VARCHAR(200) DEFAULT NULL COMMENT '模块名称',
    `ButtonName` VARCHAR(100) DEFAULT NULL COMMENT '按钮名称',
    `Content` TEXT DEFAULT NULL COMMENT '操作内容',
    `Result` TEXT DEFAULT NULL COMMENT '操作结果',
    `Url` VARCHAR(500) DEFAULT NULL COMMENT '请求链接',
    `IP` VARCHAR(50) DEFAULT NULL COMMENT '请求IP',
    `WorkStationName` VARCHAR(200) DEFAULT NULL COMMENT '工作站名称',
    `Method` VARCHAR(10) DEFAULT NULL COMMENT '请求方法（GET/POST等）',
    `Params` TEXT DEFAULT NULL COMMENT '请求参数',
    `IsEnable` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否启用',
    `IsDelete` BIT(1) NOT NULL DEFAULT b'0' COMMENT '软删除',
    `CreateTime` DATETIME DEFAULT NULL COMMENT '创建时间（日志发生时间）',
    `CreateUserCode` CHAR(36) DEFAULT NULL COMMENT '创建人Code（冗余）',
    `ModifyTime` DATETIME DEFAULT NULL COMMENT '修改时间',
    `ModifyUserCode` CHAR(36) DEFAULT NULL COMMENT '修改人Code',
    PRIMARY KEY (`Code`),
    KEY `idx_type` (`Type`),
    KEY `idx_username` (`UserName`),
    KEY `idx_createtime` (`CreateTime`),
    KEY `idx_ip` (`IP`),
    KEY `idx_isdelete` (`IsDelete`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='通用日志表';



-- ======================================================
-- 测试数据
-- ======================================================

-- 1. 角色数据（含层级）
INSERT INTO `SYS_Role` (`Code`, `Name`, `SuperiorRoleCode`, `Description`, `Level`, `IsEnable`, `CreateTime`) VALUES
('role_duty', '担当', NULL, '普通员工', 1, 1, NOW()),
('role_group', '组长', 'role_duty', '小组负责人', 2, 1, NOW()),
('role_class', '班长', 'role_group', '班负责人', 3, 1, NOW()),
('role_leader', 'Leader', 'role_section', '虚拟组负责人', 3, 1, NOW()),
('role_section', '课长', 'role_manager', '课负责人', 4, 1, NOW()),
('role_manager', '部长', 'role_head', '部负责人', 5, 1, NOW()),
('role_head', '本部长', 'role_gm', '本部负责人', 6, 1, NOW()),
('role_gm', '总经理', NULL, '公司总经理', 7, 1, NOW()),
('role_auditor', '日志审计员', NULL, '等保审计', 99, 1, NOW()),
('role_admin', '管理员', NULL, '系统管理员', 100, 1, NOW());

-- 更新上级角色引用
UPDATE SYS_Role SET SuperiorRoleCode = 'role_group' WHERE Code = 'role_duty';
UPDATE SYS_Role SET SuperiorRoleCode = 'role_class' WHERE Code = 'role_group';
UPDATE SYS_Role SET SuperiorRoleCode = 'role_section' WHERE Code = 'role_class';
UPDATE SYS_Role SET SuperiorRoleCode = 'role_section' WHERE Code = 'role_leader';
UPDATE SYS_Role SET SuperiorRoleCode = 'role_manager' WHERE Code = 'role_section';
UPDATE SYS_Role SET SuperiorRoleCode = 'role_head' WHERE Code = 'role_manager';
UPDATE SYS_Role SET SuperiorRoleCode = 'role_gm' WHERE Code = 'role_head';

-- 2. 组织架构数据（仅核心示例，可按需扩展）
INSERT INTO `SYS_Org` (`Code`, `ParentCode`, `Name`, `OrgType`, `Path`, `Level`, `Sort`, `IsVirtual`, `IsEnable`, `CreateTime`) VALUES
('org_comp', NULL, '东方集团', '公司', '/org_comp/', 0, 1, 0, 1, NOW()),
('org_hq1', 'org_comp', 'P・IJS・MS事业推进本部', '本部', '/org_comp/org_hq1/', 1, 1, 0, 1, NOW()),
('org_depSYS_mold', 'org_hq1', '模具制造部', '部', '/org_comp/org_hq1/org_depSYS_mold/', 2, 1, 0, 1, NOW()),
('org_section_mold', 'org_depSYS_mold', '模具制造课', '课', '/org_comp/org_hq1/org_depSYS_mold/org_section_mold/', 3, 1, 0, 1, NOW()),
('org_class_mold1', 'org_section_mold', '模具一班', '班', '/org_comp/org_hq1/org_depSYS_mold/org_section_mold/org_class_mold1/', 4, 1, 0, 1, NOW()),
('org_group_mold1a', 'org_class_mold1', '模具一组', '组', '/org_comp/org_hq1/org_depSYS_mold/org_section_mold/org_class_mold1/org_group_mold1a/', 5, 1, 0, 1, NOW()),
('org_virtual_a', 'org_section_mold', '虚拟项目组A', '虚拟组', '/org_comp/org_hq1/org_depSYS_mold/org_section_mold/org_virtual_a/', 4, 99, 1, 1, NOW());

-- 3. 用户数据
INSERT INTO `SYS_User` (`Code`, `Name`, `FullName`, `Email`, `Phone`, `IsActive`, `IsEnable`, `CreateTime`) VALUES
('user_sys', 'sys_user', '系统管理员', 'sys@dongfang.com', '13800000000', 1, 1, NOW()),
('user_log', 'log', '日志审计员', 'log@dongfang.com', '13800000001', 1, 1, NOW()),
('user_gm', 'zhang_gm', '张总经理', 'gm@dongfang.com', '13800000010', 1, 1, NOW()),
('user_head', 'li_head', '李本部长', 'li.head@dongfang.com', '13800000011', 1, 1, NOW()),
('user_manager', 'feng_manager', '冯部长', 'feng.manager@dongfang.com', '13800000020', 1, 1, NOW()),
('user_section', 'jiang_section', '蒋课长', 'jiang.section@dongfang.com', '13800000030', 1, 1, NOW()),
('user_class', 'xu_class', '许班长', 'xu.class@dongfang.com', '13800000040', 1, 1, NOW()),
('user_group', 'shi_group', '施组长', 'shi.group@dongfang.com', '13800000050', 1, 1, NOW()),
('user_duty_normal', 'liu_duty', '刘担当', 'liu.duty@dongfang.com', '13800000060', 1, 1, NOW()),
('user_leader_zhu', 'zhu_leader', '朱Leader', 'zhu.leader@dongfang.com', '13800000070', 1, 1, NOW()),
('user_duty_luo', 'luo_duty', '罗担当', 'luo.duty@dongfang.com', '13800000080', 1, 1, NOW());

-- 4. 用户角色分配
INSERT INTO `SYS_UserRoleOrg` (`Code`, `UserCode`, `RoleCode`, `OrgCode`, `IsPrimary`, `IsEnable`, `CreateTime`) VALUES
(UUID(), 'user_sys', 'role_admin', 'org_comp', 1, 1, NOW()),
(UUID(), 'user_log', 'role_auditor', 'org_comp', 1, 1, NOW()),
(UUID(), 'user_gm', 'role_gm', 'org_comp', 1, 1, NOW()),
(UUID(), 'user_head', 'role_head', 'org_hq1', 1, 1, NOW()),
(UUID(), 'user_manager', 'role_manager', 'org_depSYS_mold', 1, 1, NOW()),
(UUID(), 'user_section', 'role_section', 'org_section_mold', 1, 1, NOW()),
(UUID(), 'user_class', 'role_class', 'org_class_mold1', 1, 1, NOW()),
(UUID(), 'user_group', 'role_group', 'org_group_mold1a', 1, 1, NOW()),
(UUID(), 'user_duty_normal', 'role_duty', 'org_group_mold1a', 1, 1, NOW()),
(UUID(), 'user_leader_zhu', 'role_leader', 'org_virtual_a', 1, 1, NOW()),
(UUID(), 'user_duty_luo', 'role_duty', 'org_virtual_a', 1, 1, NOW());

-- 5. 菜单数据（示例模块）
-- 一级菜单
INSERT INTO `SYS_Route` (`Code`, `ParentCode`, `Path`, `Component`, `Name`, `Hidden`, `MetaTitle`, `MetaIcon`, `Sort`, `IsEnable`, `CreateTime`) VALUES
('route_system', NULL, '/system', 'layout/Layout', 'System', 0, '系统管理', 'setting', 10, 1, NOW()),
('route_part', NULL, '/part', 'layout/Layout', 'Part', 0, '部品管理', 'component', 20, 1, NOW()),
('route_mold', NULL, '/mold', 'layout/Layout', 'Mold', 0, '模具管理', 'tools', 30, 1, NOW()),
('route_delivery_change', NULL, '/delivery-change', 'layout/Layout', 'DeliveryChange', 0, '纳期变更', 'calendar', 40, 1, NOW()),
('route_dependency_termination', NULL, '/dependency-termination', 'layout/Layout', 'DependencyTermination', 0, '依赖终止', 'link', 50, 1, NOW()),
('route_supplier_process', NULL, '/supplier-process', 'layout/Layout', 'SupplierProcess', 0, '供应商处理', 'user', 60, 1, NOW());

-- 系统管理子菜单
INSERT INTO `SYS_Route` (`Code`, `ParentCode`, `Path`, `Component`, `Name`, `Hidden`, `MetaTitle`, `MetaIcon`, `Sort`, `IsEnable`, `CreateTime`) VALUES
('route_system_dict', 'route_system', 'dict', 'views/system/dict/index', 'Dict', 0, '字典管理', 'list', 1, 1, NOW()),
('route_system_user', 'route_system', 'user', 'views/system/user/index', 'User', 0, '用户管理', 'user', 2, 1, NOW()),
('route_system_menu', 'route_system', 'menu', 'views/system/menu/index', 'Menu', 0, '菜单管理', 'menu', 3, 1, NOW()),
('route_system_log', 'route_system', 'log', 'views/system/log/index', 'Log', 0, '日志管理', 'document', 4, 1, NOW()),
('route_system_role', 'route_system', 'role', 'views/system/role/index', 'Role', 0, '角色管理', 'role', 5, 1, NOW()),
('route_system_button', 'route_system', 'button', 'views/system/button/index', 'Button', 0, '按钮管理', 'edit', 6, 1, NOW()),
('route_system_org', 'route_system', 'org', 'views/system/org/index', 'Org', 0, '组织架构管理', 'tree', 7, 1, NOW());

-- 部品管理子菜单
INSERT INTO `SYS_Route` (`Code`, `ParentCode`, `Path`, `Component`, `Name`, `Hidden`, `MetaTitle`, `MetaIcon`, `Sort`, `IsEnable`, `CreateTime`) VALUES
('route_part_kaifeng', 'route_part', 'kaifeng', 'views/part/kaifeng/index', 'PartKaifeng', 0, '开封', 'open', 1, 1, NOW()),
('route_part_quotation', 'route_part', 'quotation', 'views/part/quotation/index', 'PartQuotation', 0, '报价依赖', 'money', 2, 1, NOW()),
('route_part_supplier', 'route_part', 'supplier', 'views/part/supplier/index', 'PartSupplier', 0, '供应商管理', 'office-building', 3, 1, NOW());

-- 模具管理子菜单
INSERT INTO `SYS_Route` (`Code`, `ParentCode`, `Path`, `Component`, `Name`, `Hidden`, `MetaTitle`, `MetaIcon`, `Sort`, `IsEnable`, `CreateTime`) VALUES
('route_mold_quotation', 'route_mold', 'quotation', 'views/mold/quotation/index', 'MoldQuotation', 0, '报价依赖', 'money', 1, 1, NOW()),
('route_mold_supplier', 'route_mold', 'supplier', 'views/mold/supplier/index', 'MoldSupplier', 0, '供应商管理', 'office-building', 2, 1, NOW()),
('route_mold_kaifeng', 'route_mold', 'kaifeng', 'views/mold/kaifeng/index', 'MoldKaifeng', 0, '开封', 'open', 3, 1, NOW());

-- 纳期变更子菜单
INSERT INTO `SYS_Route` (`Code`, `ParentCode`, `Path`, `Component`, `Name`, `Hidden`, `MetaTitle`, `MetaIcon`, `Sort`, `IsEnable`, `CreateTime`) VALUES
('route_delivery_change_approved', 'route_delivery_change', 'approved', 'views/delivery-change/approved/index', 'DeliveryChangeApproved', 0, '已审核', 'success', 1, 1, NOW()),
('route_delivery_change_pending', 'route_delivery_change', 'pending', 'views/delivery-change/pending/index', 'DeliveryChangePending', 0, '待审核', 'warning', 2, 1, NOW());

-- 依赖终止子菜单
INSERT INTO `SYS_Route` (`Code`, `ParentCode`, `Path`, `Component`, `Name`, `Hidden`, `MetaTitle`, `MetaIcon`, `Sort`, `IsEnable`, `CreateTime`) VALUES
('route_dependency_termination_approved', 'route_dependency_termination', 'approved', 'views/dependency-termination/approved/index', 'DependencyTerminationApproved', 0, '已审核', 'success', 1, 1, NOW()),
('route_dependency_termination_pending', 'route_dependency_termination', 'pending', 'views/dependency-termination/pending/index', 'DependencyTerminationPending', 0, '待审核', 'warning', 2, 1, NOW());

-- 供应商处理子菜单
INSERT INTO `SYS_Route` (`Code`, `ParentCode`, `Path`, `Component`, `Name`, `Hidden`, `MetaTitle`, `MetaIcon`, `Sort`, `IsEnable`, `CreateTime`) VALUES
('route_supplier_process_history', 'route_supplier_process', 'history', 'views/supplier-process/history/index', 'SupplierProcessHistory', 0, '历史报价', 'time', 1, 1, NOW()),
('route_supplier_process_quotation', 'route_supplier_process', 'quotation', 'views/supplier-process/quotation/index', 'SupplierProcessQuotation', 0, '报价', 'money', 2, 1, NOW());

-- 6. 按钮数据（示例）
-- 通用函数：为指定菜单批量插入按钮
DELIMITER $$
CREATE PROCEDURE InsertButtonsForMenu(IN p_RouteCode CHAR(36))
BEGIN
    -- 新增
    INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
    VALUES (UUID(), 'Add', p_RouteCode, '新增', 'handleAdd', 'primary', 1, 1, 1, NOW());
    -- 编辑
    INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
    VALUES (UUID(), 'Edit', p_RouteCode, '编辑', 'handleEdit', 'primary', 2, 2, 1, NOW());
    -- 删除
    INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
    VALUES (UUID(), 'Delete', p_RouteCode, '删除', 'handleDelete', 'danger', 2, 3, 1, NOW());
    -- 查看
    INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
    VALUES (UUID(), 'View', p_RouteCode, '查看', 'handleView', 'info', 1, 4, 1, NOW());
    -- 导出
    INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
    VALUES (UUID(), 'Export', p_RouteCode, '导出', 'handleExport', 'default', 1, 5, 1, NOW());
END$$
DELIMITER ;

-- 为每个叶子菜单调用存储过程（注意：系统管理的子菜单也是叶子）
CALL InsertButtonsForMenu('route_system_dict');
CALL InsertButtonsForMenu('route_system_user');
CALL InsertButtonsForMenu('route_system_menu');
CALL InsertButtonsForMenu('route_system_log');
CALL InsertButtonsForMenu('route_system_role');
CALL InsertButtonsForMenu('route_system_button');
CALL InsertButtonsForMenu('route_system_org');
CALL InsertButtonsForMenu('route_part_kaifeng');
CALL InsertButtonsForMenu('route_part_quotation');
CALL InsertButtonsForMenu('route_part_supplier');
CALL InsertButtonsForMenu('route_mold_quotation');
CALL InsertButtonsForMenu('route_mold_supplier');
CALL InsertButtonsForMenu('route_mold_kaifeng');
CALL InsertButtonsForMenu('route_delivery_change_approved');
CALL InsertButtonsForMenu('route_delivery_change_pending');
CALL InsertButtonsForMenu('route_dependency_termination_approved');
CALL InsertButtonsForMenu('route_dependency_termination_pending');
CALL InsertButtonsForMenu('route_supplier_process_history');
CALL InsertButtonsForMenu('route_supplier_process_quotation');


-- 对于“报价依赖”和“纳期变更”等流程性菜单，额外添加提交、审核按钮
-- 部品报价依赖额外按钮
INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
VALUES (UUID(), 'Submit', 'route_part_quotation', '提交', 'handleSubmit', 'success', 1, 6, 1, NOW());
INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
VALUES (UUID(), 'Approve', 'route_part_quotation', '审核', 'handleApprove', 'warning', 2, 7, 1, NOW());

-- 模具报价依赖额外按钮
INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
VALUES (UUID(), 'Submit', 'route_mold_quotation', '提交', 'handleSubmit', 'success', 1, 6, 1, NOW());
INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
VALUES (UUID(), 'Approve', 'route_mold_quotation', '审核', 'handleApprove', 'warning', 2, 7, 1, NOW());

-- 纳期变更待审核页面的审核按钮
INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
VALUES (UUID(), 'Approve', 'route_delivery_change_pending', '通过', 'handleApprove', 'success', 1, 1, 1, NOW());
INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
VALUES (UUID(), 'Reject', 'route_delivery_change_pending', '驳回', 'handleReject', 'danger', 1, 2, 1, NOW());

-- 依赖终止待审核页面的审核按钮
INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
VALUES (UUID(), 'Approve', 'route_dependency_termination_pending', '通过', 'handleApprove', 'success', 1, 1, 1, NOW());
INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
VALUES (UUID(), 'Reject', 'route_dependency_termination_pending', '驳回', 'handleReject', 'danger', 1, 2, 1, NOW());

-- 供应商处理报价页面添加提交按钮
INSERT INTO SYS_Button (Code, ButtonKey, RouteCode, Name, Event, StyleType, Type, Sort, IsEnable, CreateTime)
VALUES (UUID(), 'SubmitQuotation', 'route_supplier_process_quotation', '提交报价', 'handleSubmitQuotation', 'primary', 1, 1, 1, NOW());

-- 删除存储过程
DROP PROCEDURE IF EXISTS InsertButtonsForMenu;

-- 7. 菜单权限分配（角色授权示例）
-- 1. 总经理（role_gm）：所有菜单权限
INSERT INTO SYS_MenuPermission (Code, RouteCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), r.Code, 'Role', 'role_gm', 1, NOW()
FROM SYS_Route r WHERE r.IsEnable = 1;

-- 总经理拥有所有按钮权限（通过角色授权）
INSERT INTO SYS_ButtonPermission (Code, ButtonCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), b.Code, 'Role', 'role_gm', 1, NOW()
FROM SYS_Button b;

-- 2. 本部长（role_head）：拥有部品管理、模具管理、纳期变更、依赖终止、供应商处理模块，以及系统管理中的组织架构管理
INSERT INTO SYS_MenuPermission (Code, RouteCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), r.Code, 'Role', 'role_head', 1, NOW()
FROM SYS_Route r WHERE r.Code IN (
    'route_part', 'route_part_kaifeng', 'route_part_quotation', 'route_part_supplier',
    'route_mold', 'route_mold_quotation', 'route_mold_supplier', 'route_mold_kaifeng',
    'route_delivery_change', 'route_delivery_change_approved', 'route_delivery_change_pending',
    'route_dependency_termination', 'route_dependency_termination_approved', 'route_dependency_termination_pending',
    'route_supplier_process', 'route_supplier_process_history', 'route_supplier_process_quotation',
    'route_system_org'   -- 允许查看组织架构
);

-- 本部长的按钮权限：仅查看、导出、提交（无新增、编辑、删除）
INSERT INTO SYS_ButtonPermission (Code, ButtonCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), b.Code, 'Role', 'role_head', 1, NOW()
FROM SYS_Button b
WHERE b.ButtonKey IN ('View', 'Export', 'Submit', 'Approve', 'Reject', 'SubmitQuotation')
  AND b.RouteCode IN (
      SELECT Code FROM SYS_Route WHERE Code IN (
          'route_part_kaifeng', 'route_part_quotation', 'route_part_supplier',
          'route_mold_kaifeng', 'route_mold_quotation', 'route_mold_supplier',
          'route_delivery_change_approved', 'route_delivery_change_pending',
          'route_dependency_termination_approved', 'route_dependency_termination_pending',
          'route_supplier_process_history', 'route_supplier_process_quotation'
      )
  );

-- 3. 部长（role_manager）：仅拥有部品管理和模具管理的查看、导出权限，以及纳期变更待审核的审核按钮
INSERT INTO SYS_MenuPermission (Code, RouteCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), r.Code, 'Role', 'role_manager', 1, NOW()
FROM SYS_Route r WHERE r.Code IN (
    'route_part_kaifeng', 'route_part_quotation', 'route_part_supplier',
    'route_mold_kaifeng', 'route_mold_quotation', 'route_mold_supplier',
    'route_delivery_change_pending'
);

-- 部长的按钮权限：仅查看、导出（报价依赖可以提交，但不可审核）
INSERT INTO SYS_ButtonPermission (Code, ButtonCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), b.Code, 'Role', 'role_manager', 1, NOW()
FROM SYS_Button b
WHERE b.ButtonKey IN ('View', 'Export', 'Submit')
  AND b.RouteCode IN (
      SELECT Code FROM SYS_Route WHERE Code IN (
          'route_part_kaifeng', 'route_part_quotation', 'route_part_supplier',
          'route_mold_kaifeng', 'route_mold_quotation', 'route_mold_supplier'
      )
  );

-- 部长对纳期变更待审核拥有审核权限
INSERT INTO SYS_ButtonPermission (Code, ButtonCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), b.Code, 'Role', 'role_manager', 1, NOW()
FROM SYS_Button b
WHERE b.ButtonKey IN ('Approve', 'Reject')
  AND b.RouteCode = 'route_delivery_change_pending';

-- 4. 课长（role_section）：仅拥有部品管理中的报价依赖查看和提交，以及模具管理中的报价依赖查看
INSERT INTO SYS_MenuPermission (Code, RouteCode, SubjectType, SubjectCode, IsGranted, CreateTime)
VALUES 
(UUID(), 'route_part_quotation', 'Role', 'role_section', 1, NOW()),
(UUID(), 'route_mold_quotation', 'Role', 'role_section', 1, NOW());

INSERT INTO SYS_ButtonPermission (Code, ButtonCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), b.Code, 'Role', 'role_section', 1, NOW()
FROM SYS_Button b
WHERE b.ButtonKey IN ('View', 'Submit')
  AND b.RouteCode IN ('route_part_quotation', 'route_mold_quotation');

-- 5. 担当（role_duty）：仅拥有部品报价依赖的查看权限（本人数据权限将另外通过数据范围控制）
INSERT INTO SYS_MenuPermission (Code, RouteCode, SubjectType, SubjectCode, IsGranted, CreateTime)
VALUES (UUID(), 'route_part_quotation', 'Role', 'role_duty', 1, NOW());

INSERT INTO SYS_ButtonPermission (Code, ButtonCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), b.Code, 'Role', 'role_duty', 1, NOW()
FROM SYS_Button b
WHERE b.ButtonKey = 'View' AND b.RouteCode = 'route_part_quotation';

-- 6. 日志审计员（role_auditor）：仅拥有系统管理的日志管理菜单
INSERT INTO SYS_MenuPermission (Code, RouteCode, SubjectType, SubjectCode, IsGranted, CreateTime)
VALUES (UUID(), 'route_system_log', 'Role', 'role_auditor', 1, NOW());

INSERT INTO SYS_ButtonPermission (Code, ButtonCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), b.Code, 'Role', 'role_auditor', 1, NOW()
FROM SYS_Button b
WHERE b.ButtonKey = 'View' AND b.RouteCode = 'route_system_log';

-- 7. 管理员（role_admin）：拥有所有系统管理子菜单（不包含业务菜单）
INSERT INTO SYS_MenuPermission (Code, RouteCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), r.Code, 'Role', 'role_admin', 1, NOW()
FROM SYS_Route r
WHERE r.Code LIKE 'route_system%' AND r.Code != 'route_system';

-- 管理员拥有系统管理模块的所有按钮
INSERT INTO SYS_ButtonPermission (Code, ButtonCode, SubjectType, SubjectCode, IsGranted, CreateTime)
SELECT UUID(), b.Code, 'Role', 'role_admin', 1, NOW()
FROM SYS_Button b
WHERE b.RouteCode LIKE 'route_system%';

-- ======================================================
-- 权限查询示例（获取用户 user_gm 的所有有效菜单权限）
-- ======================================================
-- 思路：合并三种授权来源（用户直接授权、角色授权、组织授权），并去重。
-- 以下查询获取 user_gm 的菜单权限（包括其所属角色以及直接授权）
SELECT DISTINCT r.Code AS RouteCode, r.Path, r.Component, r.Name, r.MetaTitle, r.MetaIcon, r.Sort
FROM SYS_Route r
WHERE r.IsEnable = 1 AND r.IsDelete = 0
AND EXISTS (
    -- 菜单权限存在且授权
    SELECT 1 FROM SYS_MenuPermission mp
    WHERE mp.RouteCode = r.Code
    AND mp.IsGranted = 1
    AND mp.IsDelete = 0
    AND (
        -- 直接授权给用户
        (mp.SubjectType = 'User' AND mp.SubjectCode = 'user_gm')
        OR
        -- 授权给用户所属的角色（在任一任职组织下的角色）
        (mp.SubjectType = 'Role' AND mp.SubjectCode IN (
            SELECT uro.RoleCode FROM SYS_UserRoleOrg uro
            WHERE uro.UserCode = 'user_gm' AND uro.IsEnable = 1 AND uro.IsDelete = 0
        ))
        OR
        -- 授权给用户任职的组织（及其父级组织，可根据需求递归）
        (mp.SubjectType = 'Org' AND mp.SubjectCode IN (
            SELECT uro.OrgCode FROM SYS_UserRoleOrg uro
            WHERE uro.UserCode = 'user_gm' AND uro.IsEnable = 1 AND uro.IsDelete = 0
        ))
    )
);


-- 查询总经理可见的所有菜单（应包含所有）
SELECT DISTINCT r.MetaTitle, r.Path
FROM SYS_Route rsys_button
WHERE r.Code IN (
    SELECT mp.RouteCode FROM SYS_MenuPermission mp
    WHERE mp.SubjectType = 'Role' AND mp.SubjectCode = 'role_gm' AND mp.IsGranted=1
);

-- 查询本部长在部品管理模块下拥有的按钮
SELECT b.Name, b.ButtonKey
FROM SYS_Button b
WHERE b.RouteCode IN ('route_part_kaifeng', 'route_part_quotation', 'route_part_supplier')
AND EXISTS (
    SELECT 1 FROM SYS_ButtonPermission bp
    WHERE bp.ButtonCode = b.Code AND bp.SubjectType = 'Role' AND bp.SubjectCode = 'role_head'
);