# the following line imports all the dragonfly stuff we'll be using -- obviously you must have Dragonfly installed
from dragonfly import *
def func():
  return

# now we make our MappingRule object with only two commands
class Example1(MappingRule):
  mapping = {
    "test":                       Function(func),
    "some words I speak":         Key("a, b, c"),
    "some words":                 Key("a, t, c"),
    "BRB Firefox":                BringApp("Firefox") +Key("a, b, c"),
    "command number two":         Text("here is a hashtag: #"),
    "documentation dragonfly":    Text("https://pythonhosted.org/dragonfly/actions.html"),
 
  }
