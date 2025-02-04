 extern "C" {
   
     void _logToiOS(const char* debugMessage) {
          NSLog(@"UNITY LOG: %@", [NSString stringWithUTF8String:debugMessage]);
     }
 }