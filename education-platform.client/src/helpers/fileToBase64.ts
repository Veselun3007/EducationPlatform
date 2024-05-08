export  function fileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        const base64String = reader.result as string;
        const base64 = base64String.replace('data:', '').replace(/^.+,/, '');
        resolve(base64);
      };
      reader.onerror = (error) => reject(error);
    });
  };