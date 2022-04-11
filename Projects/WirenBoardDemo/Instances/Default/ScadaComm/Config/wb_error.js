log("Get Wiren Board error from topic: " + topic);

if (payload) {
   if (payload === 'r') {
      setValue(0, 1); // read error
   } else if (payload === 'w') {
      setValue(0, 2); // write error
   } else if (payload === 'p') {
      setValue(0, 3); // read period miss
   } else {
      setValue(0, 4); // unknown error
   }
} else {
    setValue(0, 0); // no error
}
