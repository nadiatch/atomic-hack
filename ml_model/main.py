from langchain_community.document_loaders import TextLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain_huggingface import HuggingFaceEmbeddings
from langchain_community.vectorstores import FAISS
import torch
import numpy as np


class DocumentProcessor:
    def __init__(self, file_path: str, vector_store_path: str, model_name: str, chunk_size=700, chunk_overlap=50):
        self.file_path = file_path
        self.chunk_size = chunk_size
        self.chunk_overlap = chunk_overlap
        self.device = "cuda" if torch.cuda.is_available() else "cpu"
        self.embedding_model = HuggingFaceEmbeddings(
            model_name=model_name,
            show_progress=True,
            model_kwargs={'device': self.device}
        )
        self.vector_store = FAISS.load_local(folder_path=vector_store_path, embeddings=self.embedding_model,
                                             allow_dangerous_deserialization=True)
        self.text_chunks = []

    def load_documents(self):
        document_loader = TextLoader(file_path=self.file_path)
        documents = document_loader.load()
        return documents

    def split_documents(self, documents):
        text_splitter = RecursiveCharacterTextSplitter(
            chunk_size=self.chunk_size,
            chunk_overlap=self.chunk_overlap
        )
        self.text_chunks = text_splitter.split_documents(documents)

    def retrieve_related_texts(self, query: str, top_k=2):
        query_embedding = np.array(self.embedding_model.embed_query(query)).astype('float32').reshape(1, -1)
        distances, indices = self.vector_store.index.search(query_embedding, top_k)
        return [self.text_chunks[i] for i in indices[0] if i < len(self.text_chunks)]

    def process(self):
        documents = self.load_documents()
        self.split_documents(documents)


class QueryEngine:
    def __init__(self, document_processor):
        self.document_processor = document_processor

    def query(self, query: str, top_k=3):
        return [i.page_content for i in self.document_processor.retrieve_related_texts(query)]


# Использование классов
file_path = "/Users/Domestos/PycharmProjects/ru_rag/instr_pdf.txt"
model_name = "sberbank-ai/sbert_large_nlu_ru"
faiss_path = '/Users/Domestos/PycharmProjects/ru_rag/faiss_index'

document_processor = DocumentProcessor(file_path, faiss_path,model_name)
document_processor.process()

query_engine = QueryEngine(document_processor)
query = str(input("Введите запрос: "))
related_text_chunks = query_engine.query(query)
print(related_text_chunks)
