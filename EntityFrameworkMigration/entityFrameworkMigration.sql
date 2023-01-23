--
-- PostgreSQL database dump
--

-- Dumped from database version 13.3
-- Dumped by pg_dump version 13.3

-- Started on 2023-01-22 23:24:59


CREATE TABLE public.first_child (
    id_first_child integer NOT NULL,
    first_child_name character varying(50),
    id_parent integer NOT NULL
);
ALTER TABLE public.first_child OWNER TO postgres;


CREATE TABLE public.parent (
    id_parent integer NOT NULL,
    parent_name character varying(50)
);
ALTER TABLE public.parent OWNER TO postgres;


CREATE TABLE public.second_child (
    id_second_child integer NOT NULL,
    second_child_name character varying(50),
    id_first_child integer NOT NULL
);
ALTER TABLE public.second_child OWNER TO postgres;


CREATE TABLE public.third_child (
    id_third_child integer NOT NULL,
    third_child_name character varying(50),
    id_second_child integer NOT NULL
);
ALTER TABLE public.third_child OWNER TO postgres;


ALTER TABLE ONLY public.parent
    ADD CONSTRAINT "Parent_pkey" PRIMARY KEY (id_parent);


ALTER TABLE ONLY public.first_child
    ADD CONSTRAINT first_child_pkey PRIMARY KEY (id_first_child);


ALTER TABLE ONLY public.second_child
    ADD CONSTRAINT second_child_pkey PRIMARY KEY (id_second_child);


ALTER TABLE ONLY public.third_child
    ADD CONSTRAINT third_child_pkey PRIMARY KEY (id_third_child);


ALTER TABLE ONLY public.first_child
    ADD CONSTRAINT fk_firstchild_parent FOREIGN KEY (id_parent) REFERENCES public.parent(id_parent) NOT VALID;


ALTER TABLE ONLY public.second_child
    ADD CONSTRAINT fk_secondchild_firstchild FOREIGN KEY (id_first_child) REFERENCES public.first_child(id_first_child) NOT VALID;


ALTER TABLE ONLY public.third_child
    ADD CONSTRAINT fk_thirdchild_secondchild FOREIGN KEY (id_second_child) REFERENCES public.second_child(id_second_child) NOT VALID;


-- Completed on 2023-01-22 23:25:00

--
-- PostgreSQL database dump complete
--

