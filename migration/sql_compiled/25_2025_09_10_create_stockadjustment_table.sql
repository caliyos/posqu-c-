
//         DROP TABLE IF EXISTS public.stock_adjustment_logs;

//         CREATE TABLE public.stock_adjustment_logs (
//             id BIGSERIAL PRIMARY KEY,
//             item_id BIGINT NOT NULL,
//             adjustment_type VARCHAR(50) NOT NULL, -- contoh: INCREASE, DECREASE, CORRECTION
//             old_stock NUMERIC(18,2) NOT NULL DEFAULT 0,
//             new_stock NUMERIC(18,2) NOT NULL DEFAULT 0,
//             difference NUMERIC(18,2) NOT NULL DEFAULT 0,
//             reason TEXT,
//             reference_id BIGINT,
//             reference_table VARCHAR(50),
//             user_id BIGINT,
//             created_at TIMESTAMP(6) NOT NULL DEFAULT CURRENT_TIMESTAMP
//         );

//         -- Relasi ke tabel items
//         ALTER TABLE public.stock_adjustment_logs
//             ADD CONSTRAINT stock_adjustment_logs_item_id_fkey
//             FOREIGN KEY (item_id)
//             REFERENCES public.items (id)
//             ON DELETE CASCADE
//             ON UPDATE NO ACTION;

//         -- Relasi ke tabel users
//         ALTER TABLE public.stock_adjustment_logs
//             ADD CONSTRAINT stock_adjustment_logs_user_id_fkey
//             FOREIGN KEY (user_id)
//             REFERENCES public.users (id)
//             ON DELETE SET NULL
//             ON UPDATE NO ACTION;
//     
